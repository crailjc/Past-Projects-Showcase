	/****************************************************
 * 2016-2021 Eric Bachmann and Mike Zmuda
 * All Rights Reserved.
 * NOTICE:
 * Dissemination of this information or reproduction
 * of this material is prohibited unless prior written
 * permission is granted.
 ****************************************************/

#include "light.h"
#include "io.h"

/**
 * @fn	color ambientColor(const color &mat, const color &light)
 * @brief	Computes the ambient color produced by a single light at a single point.
 * @param	mat  	Ambient material property.
 * @param	lightAmbient	Light's ambient color.
 * @return	Ambient color.
  */

color ambientColor(const color &mat, const color &lightAmbient) {
	// Values returned will always be between 0.0 and 1.0
	return glm::clamp(mat * lightAmbient, 0.0, 1.0);
}

/**
 * @fn	color diffuseColor(const color &mat, const color &light, const dvec3 &l, const dvec3 &n)
 * @brief	Computes diffuse color produce by a single light at a single point.
 * @param	mat		 	Material.
 * @param	lightDiffuse	 	The light color.
 * @param	l		 	Light vector.
 * @param	n		 	Normal vector.
 * @return	Diffuse color.
 */

color diffuseColor(const color &mat, const color &lightDiffuse,
					const dvec3 &l, const dvec3 &n) {
	return glm::clamp((mat * lightDiffuse) * (glm::dot(l, n)), 0.0, 1.0);
	
}

/**
 * @fn	color specularColor(const color &mat, const color &light, double shininess, 
 *							const dvec3 &r, const dvec3 &v)
 * @brief	Computes specular color produce by a single light at a single point.
 * @param	mat		 	Material.
 * @param	lightSpecular	 	The light's color.
 * @param	shininess	Material shininess.
 * @param	r		 	Reflection vector.
 * @param	v		 	Viewing vector.
 * @return	Specular color.
 */

color specularColor(const color &mat, const color &lightSpecular,
					double shininess,
					const dvec3 &r, const dvec3 &v) {
	return glm::clamp((mat * lightSpecular) * glm::pow(glm::clamp(glm::dot(r, v), 0.0, 1.0),shininess), 0.0, 1.0);
}

/**
 * @fn	color totalColor(const Material &mat, const LightColor &lightColor, 
 *						const dvec3 &viewingDir, const dvec3 &normal, 
 *						const dvec3 &lightPos, const dvec3 &intersectionPt, 
 *						bool attenuationOn, const LightAttenuationParameters &ATparams)
 * @brief	Color produced by a single light at a single point.
 * @param	mat			  	Material.
 * @param	lightColor	  	The light's color.
 * @param	v	  			The v vector.
 * @param	n   		  	Normal vector.
 * @param	lightPos	  	Light position.
 * @param	intersectionPt	(x,y,z) of intersection point.
 * @param	attenuationOn 	true if attenuation is on.
 * @param	ATparams	  	Attenuation parameters.
 * @return	Color produced by a single light at a single point.
 */
 
color totalColor(const Material &mat, const LightColor &lightColor,
				const dvec3 &v, const dvec3 &n,
				const dvec3 &lightPos, const dvec3 &intersectionPt,
				bool attenuationOn, 
				const LightATParams &ATparams) {
	double atValue = 1.0;

	// If attenuation is on calculate distance and find
	// the atValue to be used for diffuse and specular
	if (attenuationOn) {
		double distance = glm::distance(lightPos, intersectionPt);
		atValue = 1 / (ATparams.constant + ATparams.linear * (distance)+ATparams.quadratic * (distance));
	}

	dvec3 lighting = glm::normalize(lightPos - intersectionPt);
	dvec3 reflection = (2.0 * glm::dot(lighting, n) * n - lighting);
	color a = ambientColor(mat.ambient, lightColor.ambient);
	color b = atValue * diffuseColor(mat.diffuse, lightColor.diffuse, lighting, n);
	color c = atValue * specularColor(mat.specular, lightColor.specular, mat.shininess, reflection, v);

	return glm::clamp((a+b+c), 0.0, 1.0);
}

/**
 * @fn	color PositionalLight::illuminate(const dvec3 &interceptWorldCoords, 
 *										const dvec3 &normal, const Material &material, 
 *										const Frame &eyeFrame, bool inShadow) const
 * @brief	Computes the color this light produces in RAYTRACING applications.
 * @param	interceptWorldCoords	(x, y, z) at the intercept point.
 * @param	normal				The normal vector.
 * @param	material			The object's material properties.
 * @param	eyeFrame			The coordinate frame of the camera.
 * @param	inShadow			true if the point is in a shadow.
 * @return	The color produced at the intercept point, given this light.
 */

color PositionalLight::illuminate(const dvec3& interceptWorldCoords,
									const dvec3& normal,
									const Material& material,
									const Frame& eyeFrame, bool inShadow) const {

	if (!isOn) {
		return black;
	} else {
		if (inShadow) {
			return ambientColor(material.ambient, this->lightColor.ambient);
		} else { 
			dvec3 v = glm::normalize(eyeFrame.origin - interceptWorldCoords);

			// If tied to world no change is needed
			if (this->isTiedToWorld) {
				return totalColor(material, this->lightColor, v, normal, this->pos, interceptWorldCoords,
					this->attenuationIsTurnedOn, this->atParams);

			// If its not tied to world change potision to be in world
			} else {
				return totalColor(material, this->lightColor, v, normal, this->actualPosition(eyeFrame), interceptWorldCoords,
					this->attenuationIsTurnedOn, this->atParams);
			}
		}
	}
	
}

/*
 * @fn PositionalLight::actualPosition(const Frame& eyeFrame) cosnt
 * @briefReturns the global world coordinated of this light 
 * @param     eyeFrame        the camera's frame
 * @return    The global world coordinated of this light. This will be the light's
 *			  raw position. Or, it will be the position relative to the camera's 
 *			  frame (transformed into the world coordinate frame)
 */
dvec3 PositionalLight::actualPosition(const Frame& eyeFrame) const {
	return isTiedToWorld ? pos : eyeFrame.toWorldCoords(pos);
}


/**
 * @fn	color SpotLight::illuminate(const dvec3 &interceptWorldCoords, 
 *									const dvec3 &normal, const Material &material, 
 *									const Frame &eyeFrame, bool inShadow) const
 * @brief	Computes the color this light produces in raytracing applications.
 * @param	interceptWorldCoords				The surface properties of the intercept point.
 * @param	normal					The normal vector.
 * @param	material			The object's material properties.
 * @param	eyeFrame			The coordinate frame of the camera.
 * @param	inShadow			true if the point is in a shadow.
 * @return	The color produced at the intercept point, given this light.
 */

color SpotLight::illuminate(const dvec3 &interceptWorldCoords,
							const dvec3 &normal,
							const Material &material,
							const Frame &eyeFrame, bool inShadow) const {
	if (inCone(this->pos, this->spotDir, this->fov, interceptWorldCoords)) {
		return PositionalLight::illuminate(interceptWorldCoords, normal, material, eyeFrame, inShadow);
	} else {
		return black;
	}
}

/**
* @fn	void setDir (double dx, double dy, double dz)
* @brief	Sets the direction of the spotlight.
* @param	dx		x component of the direction
* @param	dy		y component of the direction
* @param	dz		z component of the direction
*/

void SpotLight::setDir(double dx, double dy, double dz) {
	spotDir = glm::normalize(dvec3(dx, dy, dz));
}

/**
* @fn	bool inCone(const dvec3& spotPos, const dvec3& spotDir, double spotFOV, const dvec3& intercept)
* @brief	Determines if an intercept point falls within a spotlight's cone.
* @param	spotPos		where the spotlight is positioned
* @param	spotDir		normalized direction of spotlight's pointing direction
* @param	spotFOV		spotlight's field of view, which is 2X of the angle from the viewing axis
* @param	intercept	the position of the intercept.
*/

bool inCone(const dvec3& spotPos, const dvec3& spotDir, double spotFOV, const dvec3& intercept) {
	dvec3 l = glm::normalize(intercept - spotPos);
	double spotCosine = glm::dot(l, spotDir);
	return (spotCosine > glm::cos(spotFOV/2));
}

/**
* @fn	bool inShadow(const dvec3& lightPos, const dvec3& intercept, const dvec3& normal, const vector<VisibleIShapePtr> objects)
* @brief	Determines if an intercept point falls in a shadow.
* @param	lightPos		where the spotlight is positioned
* @param	intercept	the position of the intercept.
* @param	normal		the normal vector at the intercept point
* @param	objects		the collection of opaque objects in the scene
*/

bool inShadow(const dvec3& lightPos, const dvec3& intercept, const dvec3& normal, const vector<VisibleIShapePtr>& objects) {
	// For a point of intersection 
	//   For each light source do 
	//      Determine distance to light source
	//      Determine direction to light source (light vector)
	//      Construct Shadow feeler
    //         dvec3 shadowFeelerOrigin = IShape::movePointOffSurface(intercept, normal);
	//         dvec3 shadowFeelerDirection = normal;
	//         Ray shadowfeeler(shadowFeelerOrigin, shadowFeelerDirection);
	
	double distanceToLight = glm::distance(lightPos, intercept);
	dvec3 shadowFeelerOrigin = IShape::movePointOffSurface(intercept, normal);
	dvec3 shadowFeelerDirection = glm::normalize(lightPos - intercept);
	Ray shadowfeeler(shadowFeelerOrigin, shadowFeelerDirection);
	
	for (unsigned int i = 0; i < objects.size(); i++) {
		VisibleIShape& thisShape = *objects[i];
		HitRecord thisHit;
		thisShape.findClosestIntersection(shadowfeeler, thisHit);
		if (thisHit.t != FLT_MAX) {
			return true;
		}
	}
	return false;
}
