// Copyright Josh Crail 2020
#define MYSQLPP_MYSQL_HEADERS_BURIED
#include <mysql++/mysql++.h>
#include <string>
#include <iostream>
#include <fstream>
#include <vector>
#include <boost/algorithm/string/split.hpp>
#include <boost/algorithm/string.hpp>

void interactive() {  
    // Connect to database with: database, server, userID, password
    mysqlpp::Connection con("cse278", "localhost", "cse278", "S3rul3z");
    // Variable to build query string
    std::string qString, temp;
    // Get user input for query
    while (std::getline(std::cin, temp) && temp != "quit") { 
        // Create a query
        qString.append(" " +temp);
        if (qString.find(";") != -1) { 
            mysqlpp::Query query = con.query(); query << qString;
            try {
                // Check query is correct and Execute query
                query.parse(); mysqlpp::StoreQueryResult result = query.store();
                // Results is a 2D vector of mysqlpp::String objects.
                // Print the results.
                std::cout << "-----Query Result-----" << std::endl;
                for (size_t row = 0; row < result.size(); row++) {
                    std::string res = "| ";
                    for (size_t col = 0; col < result[row].size(); col++) {
                        res.append(result[row][col].c_str()); res.append(" | ");
                    }
                    std::cout << res << std::endl;
                }
                std::cout << "------End Result------" << std::endl;
            } catch(mysqlpp::BadQuery e) {
                std::cerr << "Query: " << qString << std::endl;
                std::cerr << "Query is not correct SQL syntax" << std::endl;
            }
            qString = "";
        }
    }
}

std::string generateLoadQuery(std::string& line) {
    // Create base insert query string
    std::string query;
    // Split file on commas
    std::vector<std::string> strVec;
    boost::split(strVec, line, boost::is_any_of(","));
    // Start building query from split files (table name)
    query.append("INSERT INTO " + strVec[0]);
    // Strings to hold attributes and values
    std::vector<std::string> attributes;
    std::vector<std::string> values;
    
    // Build attribute and value strings
    for (unsigned int i = 1; i < strVec.size(); i++) {
        attributes.push_back(strVec[i].substr(0, strVec[i].find(":")) + ", ");
        values.push_back(strVec[i].substr(strVec[i].find(":")+1) + ", ");
    }
    
    // Form full query string
    query.append(" (");
    
    for (int i = 0; i < attributes.size(); i++) {
        query.append(attributes.at(i));
    }
    query = query.substr(0, query.length()-2);
    query.append(") VALUES (");
    
    for (int i = 0; i < values.size(); i++) {
        query.append(values.at(i));
    }
    query = query.substr(0, query.length()-2);
    query.append(");");

    // return query;
    return query;
}

void loadData(std::string& path) {
    // Open file stream
    std::fstream loadfile; 
    loadfile.open(path);
    // Connect to database with: database, server, userID, password
    mysqlpp::Connection con("cse278", "localhost", "cse278", "S3rul3z");
    // Some necessary variables for the file IO
    std::string qString;
    int i = 1;
    
    // Read file line-by-line
    while (std::getline(loadfile, qString)) {
        // Create query string from current line
        qString = generateLoadQuery(qString);
        // Create mysql++ query
        mysqlpp::Query query = con.query();
        query << qString;
        
        try {
            // Check query is correct
            query.parse();
            // Execute Query
            mysqlpp::StoreQueryResult result = query.store();
            std::cout << "Data line " << i++ << " loaded" << std::endl; 
        } catch(mysqlpp::BadQuery e) {
            std::cerr << "Query: " << qString << std::endl;
            std::cerr << "Query is not correct SQL syntax" << std::endl;
        }
    }
    loadfile.close();
}


void parseQuery(std::vector<std::string> &strV, std::vector<std::string> &attr,
        std::vector<std::string> &vals, std::vector<std::string> &nulls, 
        std::string &key) {
    std::string temp;
    // Build attribute and value strings
    for (unsigned int i = 1; i < strV.size(); i++) {
        if (strV[i].find("not_null") != std::string::npos) {
             attr.push_back(strV[i].substr(0, strV[i].find(":")));
             temp = strV[i].substr(strV[i].find(":") + 1);
             vals.push_back(temp.substr(0, temp.find(":"))); 
             nulls.push_back("NOT NULL");
             if (temp.find("key") != std::string::npos) { 
                 key = strV[i].substr(0, strV[i].find(":"));
             }
        } else {
            attr.push_back(strV[i].substr(0, strV[i].find(":")));
            vals.push_back(strV[i].substr(strV[i].find(":")+1));
            nulls.push_back("");
        }
    }
    return;
}

std::string generateCreateQuery(std::string& line) {
    // Create base insert query string
    std::string query,  key;
    // Split file on commas
    std::vector<std::string> strV, attr, vals, nulls;
    boost::split(strV, line, boost::is_any_of(","));
    // Start building query from split files (table name)
    query.append("CREATE TABLE " + strV[0].substr(strV[0].find(":") +1) + " (");
    // Strings to hold attributes and values
    parseQuery(strV, attr, vals, nulls, key);
    
    // Form full query string
    for (int i = 0; i < attr.size(); i++) {
        if ((i+1) == attr.size()) {
            query.append(attr.at(i) + " " + vals.at(i) + " " + nulls.at(i));
            if (key != "") { query.append(" ,PRIMARY KEY (" + key + ")"); }
        } else {
            query.append(attr.at(i) + " " + vals.at(i) + " " 
                + nulls.at(i) + ", ");
        }
      // return query
    } query.append(");"); return query;
}

void createTable(std::string& path) {
    // Open file stream
    std::fstream loadfile;
    loadfile.open(path);
    // Connect to database with: database, server, userID, password
    mysqlpp::Connection con("cse278", "localhost", "cse278", "S3rul3z");
    
    std::string qString, fileName;
    // Read file line-by-line
    while (std::getline(loadfile, qString)) {
        fileName = qString.substr(qString.find(":") +1, qString.find(",") 
                - (qString.find(":") +1));
        // Create query string from current line
        qString = generateCreateQuery(qString);
        // Create mysql++ query
        mysqlpp::Query query = con.query();
        query << qString;
        
        try {
            // Check query is correct
            query.parse();
            // Execute Query
            mysqlpp::StoreQueryResult result = query.store();
            std::cout << "Table " << fileName << " Created" << std::endl; 
        } catch(mysqlpp::BadQuery e) {
            std::cerr << "Query: " << qString << std::endl;
            std::cerr << "Query is not correct SQL syntax" << std::endl;
        }
    }
    loadfile.close();  
}

void dropTable(std::string table) {
    // Connect to database with: database, server, userID, password
    mysqlpp::Connection con("cse278", "localhost", "cse278", "S3rul3z");
    std::string qString;
    
    qString.append("DROP TABLE ");
    qString.append(table);
    
    mysqlpp::Query query = con.query(); query << qString;
    
    try {
        // Check query is correct
        query.parse();
        // Execute Query
        mysqlpp::StoreQueryResult result = query.store();
        std::cout << "Table " << table << " Dropped" << std::endl; 
    } catch(mysqlpp::BadQuery e) {
        std::cerr << "Query: " << qString << std::endl;
        std::cerr << "Query is not correct SQL syntax" << std::endl;
    }
}

void writeToFile(std::string& path1, std::string& path2) {
    // Open file stream
    std::fstream loadfile;
    std::ofstream outfile;
    loadfile.open(path1);
    outfile.open(path2);
    // Connect to database with: database, server, userID, password
    mysqlpp::Connection con("cse278", "localhost", "cse278", "S3rul3z");
    // Some necessary variables for the file IO
    std::string qString;
    
    while (std::getline(loadfile, qString)) {
        // Create query string from current line
        // Create mysql++ query
        mysqlpp::Query query = con.query();
        query << qString;
            try {
                // Check query is correct and Execute query
                query.parse(); mysqlpp::StoreQueryResult result = query.store();
                // Results is a 2D vector of mysqlpp::String objects.
                // Print the results.
                if (result.size() != 0) {
                    outfile << "-----Query Result-----" << std::endl;
                    for (size_t row = 0; row < result.size(); row++) {
                        std::string res = "| ";
                        for (size_t col = 0; col < result[row].size(); col++) {
                            res.append(result[row][col].c_str()); 
                            res.append(" | ");
                        }
                        outfile << res << std::endl;
                    }
                    outfile << "------End Result------" << std::endl;
                }
            } catch(mysqlpp::BadQuery e) {
                std::cerr << "Query: " << qString << std::endl;
                std::cerr << "Query is not correct SQL syntax" << std::endl;
            }
            qString = "";
        }
        loadfile.close(); outfile.close();
    }

int main(int argc, char *argv[]) {
    // Ensure arguments are specified
    if (argc == 1) {
        std::cerr << "You must specify arguments" << std::endl;
        return 1;
    }

    std::string option = argv[1];

    if (option == "-I") {
        interactive();
    } else if (option == "-L" && argc == 3) {
        std::string path = argv[2];
        loadData(path);
    } else if (option == "-C" && argc == 3) {
        std::string path = argv[2];
        createTable(path);
    } else if (option == "-D" && argc == 3) {
        dropTable(argv[2]);
    } else if (option == "-W" && argc == 4) {
        std::string path1 = argv[2], path2 = argv[3];
        writeToFile(path1, path2);
    } else {
        std::cerr << "Invalid input" << std::endl;
        return 1;
    }

    // All done
    return 0; 
}
