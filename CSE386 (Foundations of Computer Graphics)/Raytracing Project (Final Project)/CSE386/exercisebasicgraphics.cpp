/****************************************************
 * 2016-2021 Eric Bachmann and Mike Zmuda
 * All Rights Reserved.
 * NOTICE:
 * Dissemination of this information or reproduction
 * of this material is prohibited unless prior written
 * permission is granted.
 ****************************************************/

#include <ctime>
#include <vector>
#include "defs.h"
#include "utilities.h"
#include "framebuffer.h"
#include "colorandmaterials.h"
#include "rasterization.h"

FrameBuffer frameBuffer(WINDOW_WIDTH, WINDOW_HEIGHT);

const int SZ = 51;
const int SZ2 = SZ / 2;

void closedSquare(int x, int y, color C) {
}

void closedSquare(const ivec2 &centerPt, color C) {
}

void openSquare(const ivec2 &centerPt, color C) {
}

void render() {
	frameBuffer.clearColorAndDepthBuffers();

	drawLine(frameBuffer, 0, 0, 100, 100, red);
	drawLine(frameBuffer, 100, 100, 200, 100, green);

	closedSquare(100, 150, red);
	closedSquare(ivec2(200, 150), green);
	openSquare(ivec2(300, 150), blue);

	frameBuffer.showColorBuffer();
}

void resize(int width, int height) {
	frameBuffer.setFrameBufferSize(width, height);
	glutPostRedisplay();
}
int main(int argc, char *argv[]) {
 //   graphicsInit(argc, argv, __FILE__);
 //       
	//glutDisplayFunc(render);
	//glutReshapeFunc(resize);
	//glutKeyboardFunc(keyboardUtility);
	//glutMouseFunc(mouseUtility);

	//frameBuffer.setClearColor(white);

	//glutMainLoop();

	//return 0;
	//double x = 45, y = 99;
	//std::cout << "x: " << x << " " << "y: " << y << std::endl;
	//swap(x, y);
	//std::cout << "x: " << x << " " << "y: " << y << std::endl;

	double x, y ;
	pointOnUnitCircle(PI_2,x, y);
	//std::cout << x <<", " << y << std::endl;


	//dvec2 t1(2.0, 2.0);
	//dvec2 t2(0, -2);
	//dvec2 t3(1, -1);
	//dvec2 t4(100, 200);
	//dvec2 t5(200, 373);
	//dvec2 result;

	//dvec2 d1(0, 0), d2(2, 2), d3(2, 10), d4(3, 11), d5(2,0), d6 (0, -2);
	//std::cout << directionInRadians(d1, d2) << std::endl; // 0.7853981634
	//std::cout << directionInRadians(d3, d4) << std::endl; // 0.7853981634
	//std::cout << directionInRadians(d2, d5) << std::endl; // 4.7123889804
	//std::cout << directionInRadians(d2) << std::endl; // 0.7853981634
	//std::cout << directionInRadians(d6) << std::endl; // 4.7123889804

	//std::cout << directionInRadians(0, 0, 2, 2) << std::endl; // 0.7853981634
	//std::cout << directionInRadians(2, 10, 3, 11) << std::endl; // 0.7853981634
	//std::cout << directionInRadians(2, 2, 2, 0) << std::endl; // 4.7123889804

	//result = pointOnCircle(t3, 1, -1.22173);
	//std::cout << result.x << " " << result.y << std::endl;

	// quadratic(1, 4, 3)-- > [-3, -1]
	// quadratic(1,0,0) --> [0]
	// quadratic(-4, -2, -1)


		//([1.0000 0.0000 1.0000][0.0000 1.0000 0.0000])-- > 0 Correct Good
		//([2.0000 2.0000 2.0000][4.0000 - 5.0000 1.0000])-- > 0 Correct Good
		//([0.5000 0.5000 - 1.0000][2.0000 1.0000 1.5000])-- > 0 Correct Good 
		//([-1.0000 - 1.0000 1.0000][1.0000 2.0000 2.0000])-- > 0 Correct Good 
		//([1.0000 1.0000 1.0000][0.0000 1.0000 0.0000])-- > 1 Correct Good 
		//([2.0000 2.0000 3.0000][4.0000 - 5.0000 1.0000])-- > 1 Correct
		//([-1.0000 - 2.0000 - 1.0000][-2.0000 - 1.0000 - 1.0000])-- > 1 Correct
		//([0.5000 1.5000 - 1.0000][2.0000 1.0000 1.5000])-- > 1 Correct

	//dvec3 y1(5, -2, 3), y2(-4, 5, 7);
	// dvec3 w1(-1, -2, -1), w2(-2, -1, -1);
	// dvec3 q1(.5, 1.5, -1), q2(2, 1, 1.5);
	// dvec3 z1(-1, -1, 1), z2(2, 1, 1.5); 
	// dvec3 d1(1, 1, 1), d2(0, 1, 0);

	// Zero is 90 degrees for cosBetween 1,0,1  0,1,0
	// 1 is for 0 degress for cosBetween .01, 1, .01 0,.01,0

	//std::cout << formAcuteAngle(y1, y2) << std::endl;
	// std::cout << formAcuteAngle(w1, w2) << std::endl;
	// std::cout << formAcuteAngle(q1, q2) << std::endl;
	// std::cout << formAcuteAngle(z1, z2) << std::endl;
	// std::cout << formAcuteAngle(d1, d2) << std::endl;





	


	vector<double> y1 = quadratic(-3.00000000000000000000, 4.00000000000000000000, -1.00000000000000000000);
	vector<double> y2 = quadratic(-0.10000000000000000555, 0.20000000000000001110, 0.50000000000000000000);
	std::cout << y1.at(0) << " " << y1.at(1) << std::endl;
	std::cout << y2.at(0) << " " << y2.at(1) << std::endl;









	




}