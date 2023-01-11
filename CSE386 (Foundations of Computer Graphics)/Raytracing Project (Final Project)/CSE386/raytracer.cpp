/****************************************************
 * 2016-2021 Eric Bachmann and Mike Zmuda
 * All Rights Reserved.
 * NOTICE:
 * Dissemination of this information or reproduction
 * of this material is prohibited unless prior written
 * permission is granted.
 ****************************************************/
#include "raytracer.h"
#include "ishape.h"
#include "io.h"

/**
 * @fn	RayTracer::RayTracer(const color &defa)
 * @brief	Constructs a raytracers.
 * @param	defa	The clear color.
 */

RayTracer::RayTracer(const color &defa)
	: defaultColor(defa) {
}

/**
 * @fn	void RayTracer::raytraceScene(FrameBuffer &frameBuffer, int depth, const IScene &theScene) const
 * @brief	Raytrace scene
 * @param [in,out]	frameBuffer	Framebuffer.
 * @param 		  	depth	   	The current depth of recursion.
 * @param 		  	theScene   	The scene.
 */

void RayTracer::raytraceScene(FrameBuffer &frameBuffer, int depth,
								const IScene &theScene, double left, double right, double top, double bottom) const {
	const RaytracingCamera &camera = *theScene.camera;
	const vector<VisibleIShapePtr> &objs = theScene.opaqueObjs;
	const vector<VisibleIShapePtr>& Transobjs = theScene.transparentObjs;
	const vector<PositionalLightPtr> &lights = theScene.lights;

	for (int y = 0; y < top; ++y) {
		for (int x = 0; x < right; ++x) {
			DEBUG_PIXEL = (x == xDebug && y == yDebug);
			if (DEBUG_PIXEL) {
				cout << "";
			}

			color c;
			Ray ray = camera.getRay(x +left, y + bottom);
			c = this->traceIndividualRay(ray, theScene, depth);
			frameBuffer.setColor(x + left, y + bottom, c);

			frameBuffer.showAxes(x + left, y + bottom, ray, 0.25);			// Displays R/x, G/y, B/z axes
		}
	}

	frameBuffer.showColorBuffer();
}

/**
 * @fn	color RayTracer::traceIndividualRay(const Ray &ray, 
 *											const IScene &theScene,
 *											int recursionLevel) const
 * @brief	Trace an individual ray.
 * @param	ray			  	The ray.
 * @param	theScene	  	The scene.
 * @param	recursionLevel	The recursion level.
 * @return	The color to be displayed as a result of this ray.
 */

color RayTracer::traceIndividualRay(const Ray &ray, const IScene &theScene, int recursionLevel) const {
	color c;
	HitRecord hit;
	VisibleIShape::findIntersection(ray, theScene.opaqueObjs, hit);

	// Depth is zero no recursion
	if (recursionLevel == 0) {
		c = colorHelper(ray, theScene, recursionLevel);

	// Recursion level is not zero
	} else {
		dvec3 origin = hit.interceptPt + hit.normal * EPSILON;
		dvec3 dir = ray.dir - 2 * (glm::dot(ray.dir, hit.normal)) * hit.normal;
		Ray reflectionRay(origin, dir);
		color c1 = colorHelper(ray, theScene, recursionLevel); // call the helper function
		color c2 = 0.3 * traceIndividualRay(reflectionRay, theScene, recursionLevel - 1);
		c = c1 + c2;
	}
	return c;
}

color RayTracer::colorHelper(const Ray& ray, const IScene& theScene, int recursionLevel) const{
	color c;
	HitRecord hit;
	HitRecord hitTrans;
	VisibleIShape::findIntersection(ray, theScene.opaqueObjs, hit);
	VisibleIShape::findIntersection(ray, theScene.transparentObjs, hitTrans);

	// Ray hits an opaque object
	if (hit.t != FLT_MAX) {
		dvec3 backFaceN = hit.normal;
		// Check if ray is itersecting a backface
		if (glm::dot(-ray.dir, hit.normal) < 0) {
			backFaceN = -hit.normal;
		}

		// loop througt the lights to get color
		for (int i = 0; i < theScene.lights.size(); i++) {
			PositionalLight& L = *theScene.lights[i];
			// Check to see if the lights position is in global or camera. Coordinate position should be global
			bool inS = inShadow(L.actualPosition(theScene.camera->getFrame()), hit.interceptPt, backFaceN, theScene.opaqueObjs);
			c += L.illuminate(hit.interceptPt, backFaceN, hit.material, theScene.camera->getFrame(), inS);
		}


		// Ray hits transparent object and opaque object 
		if (hitTrans.t != FLT_MAX) {
			if (hitTrans.t < hit.t) {
				c = (1 - hitTrans.material.alpha) * c + hitTrans.material.alpha * hitTrans.material.ambient;
			}
			// else do nothing
		}

		if (hit.texture != nullptr) {
			color texel = hit.texture->getPixelUV(hit.u, hit.v);
			color mix = texel * .5 + c * .5;
			return mix;
			// frameBuffer.setColor(x, y, mix);
		} else {
			return c;
			// frameBuffer.setColor(x, y, c);
		}

		// The ray hits a transparent object and not an opaque object 
	} else if (hitTrans.t != FLT_MAX) {
		c = hitTrans.material.ambient * hitTrans.material.alpha + this->defaultColor;
		return c;
		// frameBuffer.setColor(x, y, hitTrans.material.ambient * hitTrans.material.alpha + this->defaultColor);
	}
	
	if (recursionLevel == 0) {
		return this->defaultColor;
	} else {
		return black;
	}
	// else  No opaque hit nor transHit
}

