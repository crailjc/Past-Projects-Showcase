// Copyright Josh Crail 2022
#include <iostream>
#include <string>
#include <fstream>
#include <sstream>
#include <iomanip>
#include <utility>
#include <vector>
#include <unordered_map>
#include <algorithm>
#include <numeric>
#include <omp.h>
#include "PNG.h"

// It is ok to use the following namespace delarations in C++ source
// files only. They must never be used in header files.
using namespace std;
using namespace std::string_literals;
using ucharVec = const std::vector<unsigned char>;
// This alias is for a vector of character vectors
using vvInt = std::vector<std::vector<int>>;

// Method to get the PixelIndex of the images
int getPixelIndex(int row, int col, int imgWidth) {
    return (row * imgWidth + col) * 4;
} 

bool sameShade(int mainColor[3], int avgColor[3], int tol) {
    // Loop through to check the shade colors
    for (int i = 0; i < 3; i++) {
        if ((abs(mainColor[i] - avgColor[i]) >= tol))
            return false;
    }
        return true;
}

void printLocations(vvInt resultMatches, PNG& maskImg, PNG& mainImg) {
    // Here x is row, col
    for (vector<int> x : resultMatches) {
        std::cout << "sub-image matched at: " <<  x[0] 
        << ", " <<   x[1] << ", " <<
        x[0] + maskImg.getHeight() << ", " << 
        x[1] + maskImg.getWidth() << std::endl;
    }
}


void getPXmatch(int maskColor, int mainColor[3], int avgColor[3], int tol,
    int &mPx, int &misPx) {
    if (maskColor <= 0) {
        if (sameShade(mainColor, avgColor, tol)) {
            mPx++;
        } else {
            misPx++;
        }
        // Else the color is white   
        // it should not be the same shade
    } else {
        if (!sameShade(mainColor, avgColor, tol)) {
            mPx++;
        } else {
            misPx++;
        }
    }
}

void checkNetMatch(int netMatch, PNG& maskImg, PNG& mainImg, int offsetRow, 
    int offsetCol, double matchPercent, vvInt& matches) {
    if (netMatch >  (maskImg.getWidth()  *  maskImg.getHeight() 
        * (matchPercent / 100))) {
                // Check for the overlap
                // The format of x will be 
                // { {row,col}, {row, col} }
                for (vector<int> x : matches) {
                    // if the offsetRow and offsetCol are within the 
                    // region of x (row, col) then it is an overlap
                    if (abs(x[0] - offsetRow) <= maskImg.getHeight() &&
                        abs(x[1] - offsetCol) <= maskImg.getWidth()) {
                            return;
                        }
                }

                // The match is not an overlap nor already in the results
                matches.push_back({offsetRow, offsetCol});
    } 
}

// Check the region inside the mask and see if the maskcolor matches the
// average background color
int getMatch(PNG& mask, PNG& main, ucharVec& maskPix, 
    ucharVec& mainPix, int avgColor[], int tol, int offR, int offC) {
    int mPx = 0, misPx = 0, maskR = 0, maskC = 0;
    int mainC[3] = {0, 0, 0};
    for (int row = 0 + offR; row < mask.getHeight() + offR; row++, maskR++) {
        // everytime we go to a new row put the mask column back to 0
        maskC = 0;
        for (int col = 0 + offC; col < mask.getWidth() + offC; col++, maskC++) {
            // int mainindex = getPixelIndex(row, col, mainImg.getWidth());
            int mainindex = getPixelIndex(row, col, main.getWidth());
            int maskindex = getPixelIndex(maskR, maskC, mask.getWidth());
            // if the pixel in the mask is black then 
            // do the average for the pixel in the main image
            // Get the maskImg color (RGB)
            // maskC = 
            mainC[0] = mainPix[mainindex];
            mainC[1] = mainPix[mainindex + 1];
            mainC[2] = mainPix[mainindex + 2];

            // Get the number of matches and the number of mismatches
            // here mPx and misPx are passed by reference so they are updated
            getPXmatch(maskPix[maskindex], mainC, avgColor, tol, mPx, misPx);
        }   
    }
    // std::cout << mPx << " " << misPx << std::endl; 
    // Return the netmatch
    return (mPx - misPx);
}

int generateBackground(PNG& mask, PNG& main, ucharVec& maskPix,
     ucharVec& mainPix, int tolerance, int offRow, int offCol) {
    // Get the average background for the region
    int size = 0, maskRow = 0, maskCol = 0, blackPX = 0;
    int avgColor[3] = {0, 0, 0};
    for (int row = 0 + offRow; row < mask.getHeight() + offRow; 
        row++, maskRow++) {
        // std::cout << "new row" << std::endl;
        for (int col = 0 + offCol; col < mask.getWidth() + offCol; 
            col++, maskCol++) {
            int mainindex = getPixelIndex(row, col, main.getWidth());
            int maskindex = getPixelIndex(maskRow, maskCol, mask.getWidth());
            // if the pixel in the mask is black then 
            // do the average for the pixel in the main image
            // Get the maskImg color

            // Black pixel
            if (maskPix[maskindex] == 0) {
                avgColor[0] += mainPix[mainindex];
                avgColor[1] += mainPix[mainindex+1];
                avgColor[2] += mainPix[mainindex+2];
                blackPX++;
                size += 1;
            }
        }
        maskCol = 0;
    }

    // This will give us the average background color (RGB)
    avgColor[0] = avgColor[0]/(size);
    avgColor[1] = avgColor[1]/(size);
    avgColor[2] = avgColor[2]/(size);


    // std::cout << blackPX << " " << std::endl;
    // std::cout << avgColor[0] << " " << avgColor[1] << " " <<
    //    avgColor[2] << std::endl;

    // once we have our average background color we can use that with
    // our other information to get the netmatch for this region
    return getMatch(mask, main, maskPix, mainPix,
        avgColor, tolerance, offRow, offCol);
}

void checkWithResult(vvInt& result, vvInt& items) {
    long unsigned int i = 0;
    for (vector<int> x : items) {
        // here x is row, col
        while (i < result.size()) {
            if (x[0] != result[i][0] && x[1] != result[i][1]) {
                // The row col is not already in the result add it
                result.push_back({items[i][0], items[i][1]});
            }
            i++;
        }
    }
}

void generateOutputImage(PNG& main, PNG& mask, vvInt matches[128],
    const std::string& outImageFile) {
    // Thread zero is the result and everything else will be built on this
    vvInt resultMatches = matches[0];
    // combine matches and make sure that they are not already in result match
    
    // Combine all of the matches 
    for (int i = 1; i < 128; i++) {
        for (long unsigned int j = 0; j < matches[i].size(); j++) {
            (checkWithResult(resultMatches, matches[j]));
        }
    }

    // Print all of the locations where a match has occured
    printLocations(resultMatches, mask, main);

    std::cout << "Number of matches: " << resultMatches.size() << std::endl;

    // Here x is row, col
    for (vector<int> x : resultMatches) {
        // Draw horizontal lines
        for (int i = 0; (i < mask.getWidth() -1); i++) { 
            main.setRed(x[0], x[1] + i); 
            main.setRed(x[0] + mask.getHeight() -1, x[1] + i); 
        } 
        // Draw vertical lines 
        for (int i = 0; (i < mask.getHeight() -1); i++) { 
            main.setRed(x[0] + i, x[1]); 
            main.setRed(x[0] + i, x[1] + mask.getWidth() -1); 
        }
    }
    main.write(outImageFile);
}


/**
 * This is the top-level method that is called from the main method to 
 * perform the necessary image search operation. 
 * 
 * \param[in] mainImageFile The PNG image in which the specified searchImage 
 * is to be found and marked (for example, this will be "Flag_of_the_US.png")
 * 
 * \param[in] srchImageFile The PNG sub-image for which we will be searching
 * in the main image (for example, this will be "star.png" or "start_mask.png") 
 * 
 * \param[in] outImageFile The output file to which the mainImageFile file is 
 * written with search image file h×ighlighted.
 * 
 * \param[in] isMask If this flag is true then the searchImageFile should 
 * be deemed as a "mask". The default value is false.
 * 
 * \param[in] matchPercent The percentage of pixels in the mainImage and
 * searchImage that must match in order for a region in the mainImage to be
 * deemed a match.
 * 
 * \param[in] tolerance The absolute acceptable difference between each color
 * channel when comparing  
 */
void imageSearch(const std::string& mainImageFile,
                const std::string& srchImageFile, 
                const std::string& outImageFile, const bool isMask = true, 
                const int matchPercent = 75, const int tolerance = 32) {
    // Implement this method using various methods or even better
    // use an object-oriented approach.

    // Open the files to be read
    PNG main, mask;
    main.load(mainImageFile);
    mask.load(srchImageFile);
    ucharVec& mainPix = main.getBuffer();
    ucharVec& maskPix = mask.getBuffer();
    
    // Start with no offset so start at 0,0
    // int offR = 0, offC = 0, numMatches = 0;
    vvInt matches[128];
        // Move the mask over the main image and offest each time
        
    #pragma omp parallel shared(matches)
        {
            int threadID = omp_get_thread_num(), offC = 0;
            for (int offR = 0; offR + mask.getHeight() <= main.getHeight();
                offR++, offC = 0) {
                while (offC + mask.getWidth() < main.getWidth()) {
                    // get the netMatch for the the given row col pixel
                    int netMatch = generateBackground(mask, main, maskPix, 
                        mainPix, tolerance, offR, offC);
                    
                    checkNetMatch(netMatch, mask, main, 
                        offR, offC, matchPercent, matches[threadID]);


                    // go to the next column
                    offC++;
                    // break;
                }
                // Reached the width of the main image go to next row 
                // and go back to column one (both done in for loop)
            }
        }
    // Do not print in the parllel section instead print 
    // here and combine all the matches together
    // where only the matches that are not overlapping are saved
    generateOutputImage(main, mask, matches, outImageFile);
}

/**
 * The main method simply checks for command-line arguments and then calls
 * the image search method in this file.
 * 
 * \param[in] argc The number of command-line arguments. This program
 * needs at least 3 command-line arguments.
 * 
 * \param[in] argv The actual command-line arguments in the following order:
 *    1. The main PNG file in which we will be searching for sub-images
 *    2. The sub-image or mask PNG file to be searched-for
 *    3. The file to which the resulting PNG image is to be written.
 *    4. Optional: Flag (True/False) to indicate if the sub-image is a mask 
 *       (deault: false)
 *    5. Optional: Number indicating required percentage of pixels to match
 *       (default is 75)
 *    6. Optiona: A tolerance value to be specified (default: 32)
 */
int main(int argc, char *argv[]) {
    if (argc < 4) {
        // Insufficient number of required parameters.
        std::cout << "Usage: " << argv[0] << " <MainPNGfile> <SearchPNGfile> "
                  << "<OutputPNGfile> [isMaskFlag] [match-percentage] "
                  << "[tolerance]\n";
        return 1;
    }
    const std::string True("true");
    // Call the method that starts off the image search with the necessary
    // parameters.
    imageSearch(argv[1], argv[2], argv[3],       // The 3 required PNG files
        (argc > 4 ? (True == argv[4]) : true),   // Optional mask flag
        (argc > 5 ? std::stoi(argv[5]) : 75),    // Optional percentMatch
        (argc > 6 ? std::stoi(argv[6]) : 32));   // Optional tolerance

    return 0;
}

// End of source code
