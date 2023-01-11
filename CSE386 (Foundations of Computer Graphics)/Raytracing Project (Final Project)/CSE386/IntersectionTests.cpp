#include "ishape.h"
#include "io.h"

void checkEm(const char *name, const IShape& shape) {
	double sqr3 = -1.0 / glm::sqrt(3.0);
	double sqrp3 = 1.0 / glm::sqrt(3.0);
	Ray ray1(dvec3(0, 0, 0), glm::normalize(dvec3(0, 0.5, -1)));	// Viewing rays are normalized
	Ray ray2(dvec3(0, 0, 0), glm::normalize(dvec3(0, 0, -1)));      // Origin and direction
	Ray ray3(dvec3(0, 0, 0), dvec3(0, -0.5, -1));
	Ray ray4(dvec3(10, 0, 2), dvec3(sqr3, sqr3, sqr3));
	Ray ray5(dvec3(0, 0, 0), dvec3(sqrp3, sqrp3, sqrp3));

	HitRecord hit1;
	HitRecord hit2;
	HitRecord hit3;
	HitRecord hit4;
	HitRecord hit5;

	shape.findClosestIntersection(ray1, hit1);
	shape.findClosestIntersection(ray2, hit2);
	shape.findClosestIntersection(ray3, hit3);
	shape.findClosestIntersection(ray4, hit4);
	shape.findClosestIntersection(ray5, hit5);

	cout << name << endl;
	cout << "==============" << endl;
	cout << hit1.t << ' ' << hit1.interceptPt << ' '  << hit1.normal << endl;
	cout << hit2.t << ' ' << hit2.interceptPt << ' ' << hit2.normal << endl;
	cout << hit3.t << ' ' << hit3.interceptPt << ' ' << hit3.normal << endl;
	cout << hit4.t << ' ' << hit4.interceptPt << ' ' << hit4.normal << endl;
	cout << hit5.t << ' ' << hit5.interceptPt << ' ' << hit5.normal << endl;
	cout << endl;
}

int main(int argc, char* argv[]) {
	double s3 = -1.0/glm::sqrt(3.0);
	checkEm("Plane", IPlane(dvec3(0, -1, 0), dvec3(0, 1, 0)));	// normal vectors will be unit length (point on plane, normal vector)
	checkEm("Plane2", IPlane(dvec3(1, 2, 3), dvec3(s3, s3, s3)));
	checkEm("Sphere", ISphere(dvec3(0.0, 0, -1.0), 0.75)); // 
	checkEm("Shpere2", ISphere(dvec3(2, 1, 2), 1.0));
	checkEm("Disk1", IDisk(dvec3(0, 0, -1), dvec3(0, 0, 1), 1.0));
	checkEm("Disk2", IDisk(dvec3(0, 0, -10), dvec3(0, 0, 1), 1.0));

	ISphere(dvec3(0, 0, 0), 2.0);
	return 0;
}
/*
Plane
==============
3.40282e+38[0 0 0][0 0 0]
3.40282e+38[0 0 0][0 0 0]
2.23607[0 - 1 - 2][0 1 0]

Sphere
==============
0.292347[0 0.130742 - 0.261484][0 0.174322 0.984689]
0.25[0 0 - 0.25][0 0 1]
0.292347[0 - 0.130742 - 0.261484][0 - 0.174322 0.984689]

Disk1
==============
1.11803[0 0.5 - 1][0 0 1]
1[0 0 - 1][0 0 1]
1.11803[0 - 0.5 - 1][0 0 1]
Disk2
==============
3.40282e+38[0 5 - 10][0 0 1]
10[0 0 - 10][0 0 1]
3.40282e+38[0 - 5 - 10][0 0 1]
*/