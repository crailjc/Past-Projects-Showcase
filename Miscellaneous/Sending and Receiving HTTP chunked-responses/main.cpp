/** 
 * A custom web-server that can process HTTP GET requests and:
 *    1. Respond with contents of a given HTML file
 *    2. Run a specified program and return the output from the process.
 *
 * Copyright (C) 2020 raodm@miamioh.edu.
 */

#include <sys/wait.h>
#include <boost/asio.hpp>
#include <string>
#include <thread>
#include <future>
#include <vector>
#include "HTTPFile.h"
#include "ChildProcess.h"
#include "HTMLFragments.h"

// Convenience namespace to streamline the code below.
using namespace boost::asio;
using namespace boost::asio::ip;

// Forward declaration for url_decode method defined below.  We need
// this just to organize the starter code into sections that students
// should not modify (to minimize problems for students).
std::string url_decode(std::string url);

// Forward declaration for serveClient that is defined further below.
void serveClient(std::istream& is, std::ostream& os, bool);

// Forward declaration for getstat that is defined further below
void getstat(int pid, std::vector<int> &stat);

// Forward declaration for processOutput that is defined further below
void processOutput(std::vector<int> &stat, std::ostream& os, bool genChart);

/**
 * Runs the program as a server that listens to incoming connections.
 * 
 * @param port The port number on which the server should listen.
 */
void runServer(int port) {
    // Setup a server socket to accept connections on the socket
    io_service service;
    // Create end point
    tcp::endpoint myEndpoint(tcp::v4(), port);
    // Create a socket that accepts connections
    tcp::acceptor server(service, myEndpoint);
    std::cout << "Server is listening on "
              << server.local_endpoint().port()
              << " & ready to process clients...\n";
    // Process client connections one-by-one...forever
    using TcpStreamPtr = std::shared_ptr<tcp::iostream>;
    while (true) {
        // Create garbage-collected, shared object on heap so we can
        // send it to another thread and not worry about life-time of
        // the socket connection.
        TcpStreamPtr client = std::make_shared<tcp::iostream>();
        // Wait for a client to connect    
        server.accept(*client->rdbuf());
        // Process request from client on a separate background thread
        // I am using a lambda here so that I don't have to write
        // another method (no, not lazy, just simple :-)).
        std::thread thr([client](){ serveClient(*client, *client, true); });
        thr.detach();
    }
}

/**
 * This is pretty much a copy-paste from a previous exercise solution.
 * This method uses the ChildProcess class to run a program and sends
 * the program outputs as HTTP chunked-responses.
 *
 * @param cmd The command to be executed. This command should be already
 * url_decoded and is in the form "ls -la ./"
 *
 * @param os The output stream to where the output from the command is
 * to be written in chunked HTTP response format.
 * 
 * @param genChart If flag is true, then graph data is also printed.
 */
void sendCmdOutput(std::string& cmd, std::ostream& os, bool genChart) {
    // Split the command into words for processing using helper method
    // in ChildProcess.
    const StrVec args = ChildProcess::split(cmd);
    // Create the child process.
    ChildProcess cp;
    cp.forkNexecIO(args);
    
    // create vector to store the stat values
    std::vector<int> stat;
    std::thread monitorThread(getstat, cp.getPid(), std::ref(stat));
    monitorThread.join();
    // Get the outputs from the child process.
    std::istream& is = cp.getChildOutput();
    // Send the output from child process to the client as chunked response
    os << http::DefaultHttpHeaders << "text/html\r\n\r\n";
    os << std::hex << htmlStart.length() << "\n" << htmlStart << "\n";
    // Send each line as a spearate chunk.
    for (std::string line; std::getline(is, line);) {
        line += "\n";  // Add the newline that was not included by getline
        os << std::hex << line.size() << "\r\n" << line << "\r\n";
    }
    
    // It is important to wait for the process to finish. Internally,
    // the method below calls waitpid system call.
    int exitCode = cp.wait();
    const std::string line = "Exit code: " + std::to_string(exitCode) + "\n";
    os << std::hex << line.size() << "\r\n" << line << "\r\n";
    processOutput(stat, os, genChart);
    
    // Finally send the trailing end-of-stream chunk
    os << "0\r\n\n";
}

/**
 * This is a helper method that is used by the thread to get
 * the stat information from the /proc/pid/stat file over 
 * a number of seconds
 *
 * @param pid this is a integer that is used to store 
 * the pid of the process that has just forked
 * 
 * @param stat is a reference to a vector of integers that is used
 * to store information pertaining to the statistics being read 
 */
void getstat(int pid, std::vector<int> &stat) {
    int exitCode = 0;
    
    while (waitpid(pid, &exitCode, WNOHANG) == 0) {
        // sleep for 1 second.
        // Record current statistics about the process.
        sleep(1);
        std::ifstream statFile("/proc/" + std::to_string(pid) + "/stat");
        
        // Only get the stats if the file is good otherwise 
        // unwanted behavior will occur
        if (statFile.good()) {
            std::string line, temp;
            std::getline(statFile, line);
            std::stringstream word(line);
            // We could go further but we don't need anything past the 23rd 
            // element in the array
            for (int i = 0; (i < 23) && (word >> temp); i++) {    
                // get a single word at a time and put in temp var to 
                // process that stat that it has
                // word >> temp;

                // If there is a match the string will be changed into 
                // a number that then can be stored in its respective 
                // value slot
                if (i == 13) {  // Utime
                    stat.push_back(
                        std::round(std::stof(temp) / sysconf(_SC_CLK_TCK)));
                } else if (i == 14) {  // Stime
                    stat.push_back(
                        std::round(std::stof(temp) / sysconf(_SC_CLK_TCK)));
                } else if (i == 22) {  // vsize
                    stat.push_back(
                        std::round(std::stol(temp) / 1000000));
                }
            }
        }
    }
}

/**
 * This is a helper method that is used to take statistics we read and turn 
 * them into something that can be easy to understand and be formatted
 * so that the data can  passed to a webpage so that it can be read
 *
 * @param stat is a reference to an integer vector that has the data we
 * need and all we have to do is format it correctly 
 * 
 * @param os is a reference to the ostream operator this is used so that 
 * all out output is being send out through the same stream so there is no
 * potential for using the wrong stream 
 * 
 * @param genChart this is a bool that is used so that when working on a webpage
 * the data will be formatted into a JSON array that can be used to generate
 * a graph on the web page
 */
void processOutput(std::vector<int> &stat, std::ostream& os, bool genChart) {
    // String used to store the total output and keep track of character count
    std::string temp;
    temp = htmlMid1;
    int k = 1;
    // Since we collected the stats in pairs of three we
    // loop through 3 so that every line has only those three lines
    for (size_t i = 0; i < stat.size(); i+=3) {
         temp.append("       <tr><td>" + std::to_string(k++) 
                + "</td><td>" + std::to_string(stat.at(i)) + 
                "</td><td>" + std::to_string(stat.at(i+1)) + "</td><td>" +
                std::to_string(stat.at(i+2)) + "</td></tr>" + "\n");
    }
    temp.append(htmlMid2);
    // If true we will need to do some extra processing to create 
    // the JSON array that will be used to create the graph in html
    if (genChart) {
        k = 1;
        temp.append(",\n          ");
        for (size_t i = 0; i < stat.size(); i+=3) {
         temp.append("[" + std::to_string(k++) + ", " + 
            std::to_string(stat.at(i) + stat.at(i+1)) + ", " +
            std::to_string(stat.at(i+2)) + "],\n          ");
        }
        // This is used to remove the extra space and new line 
        // from the last entry in the array
        temp = temp.substr(0, temp.length() - 13);
        temp.append("]");
    }
    temp.append(htmlEnd + "\n");
    // At the end our chunk size will be the length
    // of the temp string and then we just pass the full 
    // string into the os stream to be outputted
    os << (temp.length())  << "\n" << temp;
}

//------------------------------------------------------------------
//  DO  NOT  MODIFY  CODE  BELOW  THIS  LINE
//------------------------------------------------------------------

/**
 * Process HTTP request (from first line & headers) and
 * provide suitable HTTP response back to the client.
 * 
 * @param is The input stream to read data from client.
 *
 * @param os The output stream to send data to client.
 * 
 * @param genChart If flag is true, then graph data is also printed.
 */
void serveClient(std::istream& is, std::ostream& os, bool genChart) {
    // Read headers from client and print them. This server
    // does not really process client headers
    std::string line, path;
    // Read the "GET" word and then the path.
    is >> line >> path;
    // Skip/ignore all the HTTP request & headers for now.
    while (std::getline(is, line) && (line != "\r")) {}
    // Check and dispatch the request appropriately
    if (path.find("/cgi-bin/exec?cmd=") == 0) {
        // This is a command to be processed. So use a helper method
        // to streamline the code.
        // Remove the "/cgi-bin/exec?cmd=" prefix to help process the
        // command.
        auto cmd = url_decode(path.substr(18));
        sendCmdOutput(cmd, os, genChart);
    } else if (!path.empty()) {
        // In this case we assume the user is asking for a file.  Have
        // the helper http class do the processing.
        path = "." + path;  // make path with-respect-to pwd.
        // Use the http::file helper method to send the response back
        // to the client.
        os << http::file(path);
    }
}

/** Convenience method to decode HTML/URL encoded strings.

    This method must be used to decode query string parameters
    supplied along with GET request.  This method converts URL encoded
    entities in the from %nn (where 'n' is a hexadecimal digit) to
    corresponding ASCII characters.

    \param[in] str The string to be decoded.  If the string does not
    have any URL encoded characters then this original string is
    returned.  So it is always safe to call this method!

    \return The decoded string.
*/
std::string url_decode(std::string str) {
    // Decode entities in the from "%xx"
    size_t pos = 0;
    while ((pos = str.find_first_of("%+", pos)) != std::string::npos) {
        switch (str.at(pos)) {
            case '+': str.replace(pos, 1, " ");
            break;
            case '%': {
                std::string hex = str.substr(pos + 1, 2);
                char ascii = std::stoi(hex, nullptr, 16);
                str.replace(pos, 3, 1, ascii);
            }
        }
        pos++;
    }
    return str;
}

/**
 * The main function that serves as a test harness based on
 * command-line arguments.
 *
 * \param[in] argc The number of command-line arguments.  This test
 * harness can work with zero or one command-line argument.
 *
 * \param[in] argv The actual command-line arguments.  If this is an
 * number it is assumed to be a port number.  Otherwise it is assumed
 * to be an file name that contains inputs for testing.
 */
int main(int argc, char *argv[]) {
    // Check and use first command-line argument if any as port or file
    const std::string True = "true";  // Just a sentinel value
    std::string arg = (argc > 1 ? argv[1] : "0");
    const bool genChart = (argc > 2 ? (argv[2] == True) : false);
    // Check and use a given input data file for testing.
    if (arg.find_first_not_of("1234567890") == std::string::npos) {
        // All characters are digits. So we assume this is a port
        // number and run as a standard web-server
        runServer(std::stoi(arg));
    } else {
        // In this situation, this program processes inputs from a
        // given data file for testing.  That is, instead of a
        // web-browser we just read inputs from a data file.
        std::ifstream getReq(arg);
        if (!getReq.good()) {
            std::cerr << "Unable to open " << arg << ". Aborting.\n";
            return 2;
        }
        // Have the serveClient method process the inputs from a given
        // file for testing.
        serveClient(getReq, std::cout, genChart);
    }
    // All done.
    return 0;
}
