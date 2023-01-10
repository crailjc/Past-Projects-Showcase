-- --------------------------------------------------------
-- Host:                         courseplanner.csi.miamioh.edu
-- Server version:               8.0.30-0ubuntu0.20.04.2 - (Ubuntu)
-- Server OS:                    Linux
-- HeidiSQL Version:             11.3.0.6295
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Dumping database structure for course_planner
DROP DATABASE IF EXISTS `course_planner`;
CREATE DATABASE IF NOT EXISTS `course_planner` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `course_planner`;

-- Dumping structure for procedure course_planner.AddComment
DROP PROCEDURE IF EXISTS `AddComment`;
DELIMITER //
CREATE PROCEDURE `AddComment`(
	IN `planID` INT,
	IN `content` TEXT,
	IN `personID` INT
)
    MODIFIES SQL DATA
BEGIN
INSERT INTO Comments (PlanID, Text, Date, PersonID) VALUES (planID, content, NOW(), personID);
END//
DELIMITER ;

-- Dumping structure for procedure course_planner.AddCourseToSemester
DROP PROCEDURE IF EXISTS `AddCourseToSemester`;
DELIMITER //
CREATE PROCEDURE `AddCourseToSemester`(
	IN `semesterID` INT,
	IN `courseID` INT
)
BEGIN
INSERT INTO SemesterCourse (semesterID, courseID) VALUES (semesterID, courseID);
END//
DELIMITER ;

-- Dumping structure for procedure course_planner.AddEquivalentCourse
DROP PROCEDURE IF EXISTS `AddEquivalentCourse`;
DELIMITER //
CREATE PROCEDURE `AddEquivalentCourse`(
	IN `courseID` INT,
	IN `equivCourseID` INT
)
BEGIN
INSERT INTO EquivalentCourse (courseID, equivCourseID) VALUES (courseID, equivCourse);
END//
DELIMITER ;

-- Dumping structure for procedure course_planner.AddSemesterToPlan
DROP PROCEDURE IF EXISTS `AddSemesterToPlan`;
DELIMITER //
CREATE PROCEDURE `AddSemesterToPlan`(
	IN `planID` INT,
	IN `season` INT,
	IN `currentYear` INT
)
BEGIN

END//
DELIMITER ;

-- Dumping structure for table course_planner.Comments
DROP TABLE IF EXISTS `Comments`;
CREATE TABLE IF NOT EXISTS `Comments` (
  `CommentID` int NOT NULL AUTO_INCREMENT,
  `PlanID` int NOT NULL,
  `Text` varchar(255) DEFAULT NULL,
  `Date` datetime DEFAULT NULL,
  `PersonID` int DEFAULT NULL,
  PRIMARY KEY (`CommentID`),
  KEY `PlanID` (`PlanID`),
  CONSTRAINT `Comments_ibfk_1` FOREIGN KEY (`PlanID`) REFERENCES `Plan` (`planID`)
) ENGINE=InnoDB AUTO_INCREMENT=43 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table course_planner.Comments: ~23 rows (approximately)
/*!40000 ALTER TABLE `Comments` DISABLE KEYS */;
REPLACE INTO `Comments` (`CommentID`, `PlanID`, `Text`, `Date`, `PersonID`) VALUES
	(1, 89, 'Hello there', '2021-10-31 14:03:24', 1),
	(2, 89, 'Test', '2021-10-31 15:35:05', 1),
	(3, 89, 'Test 2', '2021-10-31 15:37:00', 1),
	(4, 89, 'Test 3', '2021-10-31 15:49:27', 1),
	(5, 89, 'Test 4', '2021-10-31 15:50:06', 1),
	(9, 100, 'This is a cool cool comment', '2021-11-03 08:17:08', 1),
	(10, 100, 'Greetings', '2021-11-03 09:18:08', 1),
	(11, 100, 'Good afternoon', '2021-11-03 16:31:48', 1),
	(12, 97, 'Hi', '2021-11-03 16:46:42', 1),
	(15, 115, 'Hello', '2021-11-10 09:06:51', 5),
	(16, 115, 'This plan needs improvement', '2021-11-10 09:16:05', 4),
	(17, 115, 'This plan is a bit better, but you won\'t graduate with only 6 classses', '2021-11-10 09:19:05', 4),
	(21, 126, 'Looking to study abroad', '2021-11-11 18:48:53', 9),
	(22, 128, 'Hey', '2021-11-14 15:24:42', 9),
	(23, 128, 'Hey', '2021-11-14 19:35:07', 9),
	(24, 130, 'here\'s a comment', '2021-11-17 14:33:28', 16),
	(25, 131, 'here\'s the second plan to be famous', '2021-11-17 14:34:20', 16),
	(26, 131, 'having trouble placing courses.', '2021-11-17 14:36:43', 16),
	(28, 138, 'This is a comment', '2022-02-25 15:56:08', 11),
	(29, 139, 'How do I select courses?', '2022-02-28 16:35:29', 19),
	(38, 156, 'test', '2022-03-15 09:30:38', 20),
	(39, 157, 'Test', '2022-04-10 18:57:51', 20),
	(40, 157, 'Test', '2022-04-10 19:23:11', 20),
	(41, 157, 'This is a comment', '2022-04-10 19:24:51', 20),
	(42, 169, 'Test', '2022-04-10 20:19:00', 20);
/*!40000 ALTER TABLE `Comments` ENABLE KEYS */;

-- Dumping structure for procedure course_planner.ContainsEquivalentCourse
DROP PROCEDURE IF EXISTS `ContainsEquivalentCourse`;
DELIMITER //
CREATE PROCEDURE `ContainsEquivalentCourse`(
	IN `courseID` INT,
	IN `equivCourse` INT
)
BEGIN
SELECT COUNT(*) FROM EquivalentCourse e WHERE e.courseID = courseID AND e.equivCourseID = equivCourse;
END//
DELIMITER ;

-- Dumping structure for procedure course_planner.ContainsPerson
DROP PROCEDURE IF EXISTS `ContainsPerson`;
DELIMITER //
CREATE PROCEDURE `ContainsPerson`(
	IN `miamiID` VARCHAR(50)
)
BEGIN
SELECT COUNT(*) FROM Person p WHERE p.miamiID = miamiID;
END//
DELIMITER ;

-- Dumping structure for table course_planner.Course
DROP TABLE IF EXISTS `Course`;
CREATE TABLE IF NOT EXISTS `Course` (
  `courseID` int NOT NULL AUTO_INCREMENT,
  `departmentID` int NOT NULL,
  `courseDescription` varchar(10000) DEFAULT NULL,
  `creditHours` smallint NOT NULL,
  `courseName` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`courseID`),
  KEY `departmentID` (`departmentID`),
  CONSTRAINT `Course_ibfk_1` FOREIGN KEY (`departmentID`) REFERENCES `Department` (`departmentID`)
) ENGINE=InnoDB AUTO_INCREMENT=212 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table course_planner.Course: ~125 rows (approximately)
/*!40000 ALTER TABLE `Course` DISABLE KEYS */;
REPLACE INTO `Course` (`courseID`, `departmentID`, `courseDescription`, `creditHours`, `courseName`) VALUES
	(1, 1, 'Introduction to Computer Science and Software Engineering', 3, '102'),
	(2, 1, 'Fundamentals of Programming and Problem Solving', 3, '174'),
	(3, 1, 'Introduction to Software Engineering', 3, '201'),
	(4, 1, 'Software Construction.', 3, '211'),
	(5, 1, 'Software Engineering for User Interface and User Experience Design.', 3, '212'),
	(6, 1, 'Technology, Ethics, and Global Society.', 3, '262'),
	(7, 1, 'Special Topics.', 3, '270'),
	(8, 1, 'Object-Oriented Programming. ', 3, '271'),
	(9, 1, 'Data Abstraction and Data Structures.', 3, '274'),
	(10, 1, 'Systems I: Introduction to Systems Programming.', 3, '278'),
	(11, 2, 'Computing, Engineering & Society.', 1, '101'),
	(12, 2, 'Metal on Metal: Engineering and Globalization in Heavy Metal Music.', 3, '266'),
	(13, 3, 'Calculus I.', 5, '151'),
	(14, 3, 'Introduction to Linear Algebra.', 3, '222'),
	(15, 3, 'Elements of Discrete Mathematics.', 3, '231'),
	(16, 3, 'Differential Equations for Engineers.', 3, '245'),
	(17, 3, 'Calculus II.', 5, '249'),
	(18, 3, 'Calculus II.', 4, '251'),
	(19, 3, 'Calculus III.', 4, '252'),
	(20, 4, 'Statistics.', 4, '261'),
	(21, 5, 'Introduction to Game Studies.', 3, '211'),
	(22, 5, 'Introduction to Game Design.', 3, '212'),
	(23, 7, 'Digital Systems Design.', 4, '287'),
	(24, 8, 'Information Technology and the Intelligent Enterprise.', 3, '235'),
	(25, 9, 'Composition and Rhetoric for Second-Language Writers.', 4, '109'),
	(26, 9, 'Composition and Rhetoric.', 3, '111'),
	(27, 10, 'Principles of Microeconomics.', 3, '201'),
	(28, 10, 'Principles of Macroeconomics.', 3, '202'),
	(29, 21, 'Principles of Public Speaking.', 3, '135'),
	(30, 6, 'General Physics with Laboratory I.', 5, '191'),
	(31, 6, 'General Physics with Laboratory II.', 5, '192'),
	(32, 19, 'Biotechnology: Coming of Age in the 21st Century', 3, '101'),
	(33, 20, 'Biological Concepts: Ecology, Evolution, Genetics, and Diversity.', 4, '115'),
	(34, 20, 'Biological Concepts: Structure, Function, Cellular, and Molecular Biology.', 4, '116'),
	(35, 19, 'Environmental Biology.', 3, '121'),
	(36, 19, 'Evolution: Just a theory?.', 3, '126'),
	(37, 19, 'Plants, Humanity, and Environment.', 3, '131'),
	(38, 12, 'College Chemistry.', 3, '141'),
	(39, 12, 'College Chemistry.', 3, '142'),
	(40, 12, 'College Chemistry Laboratory.', 2, '144'),
	(41, 12, 'College Chemistry Laboratory.', 2, '145'),
	(42, 13, 'Global Design.', 3, '107'),
	(43, 13, 'Ideas in Architecture.', 3, '188'),
	(44, 13, 'History of Architecture I.', 3, '221'),
	(45, 17, 'Arts of Africa, Oceania and Native America.', 3, '162'),
	(46, 17, 'Concepts in Art.', 3, '181'),
	(47, 17, 'History of Western Art: Prehistoric-Gothic.', 3, '187'),
	(48, 11, 'Introduction to Asian/ Asian American Studies.', 3, '201'),
	(49, 11, 'Asia and Globalization.', 3, '207'),
	(50, 18, 'Psychology Of The Learner.', 3, '101'),
	(51, 18, 'Human Development and Learning in Social and Educational Contexts.', 3, '201'),
	(52, 22, 'Global Forces, Local Diversity.', 3, '101'),
	(53, 22, 'World Regional Geography: Patterns and Issues.', 3, '111'),
	(54, 23, 'Traditional Chinese Literature in English Translation.', 3, '251'),
	(55, 23, 'Modern Chinese Literature in English Translation.', 3, '252'),
	(56, 14, 'Greek Civilization in its Mediterranean Context.', 3, '101'),
	(57, 14, 'Roman Civilization.', 3, '102'),
	(58, 14, 'Introduction to Classical Mythology.', 3, '121'),
	(59, 15, 'Masterpieces of French Culture in Translation.', 3, '131'),
	(60, 16, 'World History to 1500.', 3, '197'),
	(61, 16, 'World History Since 1500.', 3, '198'),
	(62, 16, 'Genocides in the 20th Century.', 3, '231'),
	(63, 16, 'Food in History.', 3, '238'),
	(64, 16, 'Making of Modern Europe, 1450-1750.', 3, '245'),
	(65, 16, 'Latin America in the United States.', 3, '260'),
	(66, 1, 'Software Architecture & Design', 3, '311'),
	(67, 1, 'Software Quality Assurance', 3, '321'),
	(68, 1, 'Algorithms I', 3, '374'),
	(69, 1, 'Systems 2', 3, '381'),
	(70, 1, 'Mobile App Development', 3, '382'),
	(71, 1, 'Web Application Programming', 3, '383'),
	(72, 1, 'Database Systems', 3, '385'),
	(73, 1, 'Foundations of Graphics', 3, '386'),
	(74, 1, 'Game Design and Implementation', 3, '389'),
	(75, 1, 'Machine Learning', 3, '432'),
	(76, 1, 'High Performance Computing', 3, '443'),
	(77, 1, 'Senior Design Project', 2, '448'),
	(78, 1, 'Senior Design Project', 2, '449'),
	(79, 1, 'Web Services', 3, '451'),
	(80, 1, 'BioInformatic Principles', 3, '456'),
	(81, 1, 'Comparative Programming Languages', 3, '465'),
	(82, 1, 'Computer and Network Security', 3, '467'),
	(83, 1, 'Advanced Database Systems', 3, '485'),
	(84, 1, 'Intro Artifical Intelligence', 3, '486'),
	(85, 9, 'Technical Writing', 3, '313'),
	(86, 24, 'Elective Placeholder', 3, 'Perspectives or Study Abroad 2'),
	(87, 25, 'Elective Placeholder', 3, 'Arts Elective'),
	(88, 10, 'Micro/Macro', 3, '201 or 202'),
	(89, 3, 'Math elective', 3, '331'),
	(90, 3, 'Math elective', 3, '347'),
	(91, 3, 'Math elective', 3, '411'),
	(92, 3, 'Math elective', 3, '421'),
	(93, 3, 'Math elective', 3, '432'),
	(94, 3, 'Math elective', 3, '437'),
	(95, 3, 'Math elective', 3, '438'),
	(96, 3, 'Math elective', 3, '439'),
	(97, 3, 'Math elective', 3, '441'),
	(98, 3, 'Math elective', 3, '447'),
	(99, 26, 'InterCultural Elective', 3, 'Perspectives Elective'),
	(100, 26, 'Elective', 3, 'Elective'),
	(101, 27, 'Elective', 3, 'Elective'),
	(102, 28, 'Elective', 3, 'Elective'),
	(103, 29, 'Elective', 3, 'Elective'),
	(104, 4, 'Stat Elective', 3, '467'),
	(105, 4, 'Stat Elective', 3, '466'),
	(106, 4, 'Stat Elective', 3, '432'),
	(107, 4, 'Stat Elective', 3, '427'),
	(108, 4, 'Stat Elective', 3, '404'),
	(109, 4, 'Stat Elective', 3, '402'),
	(110, 4, 'Stat Elective', 3, '401'),
	(111, 4, 'Stat Elective', 3, '365'),
	(112, 4, 'Stat Elective', 3, '363'),
	(113, 4, 'Stat Elective', 3, '333'),
	(114, 1, 'CS Elective', 3, '466'),
	(115, 1, 'CS Elective', 3, '470'),
	(116, 1, 'CS Elective', 3, '471'),
	(117, 1, 'CS Elective', 3, '473'),
	(118, 1, 'CS Elective', 3, '474'),
	(119, 1, 'CS Elective', 3, '484'),
	(120, 1, 'CS Elective', 3, '488'),
	(121, 1, 'CS Elective', 3, '489'),
	(122, 1, 'CS Elective', 3, 'ELEC'),
	(123, 1, 'CS Elective', 3, '273'),
	(124, 1, 'CS Elective', 3, '311'),
	(125, 1, 'CS Elective', 3, '321'),
	(126, 1, 'CS Elective', 3, '322'),
	(127, 1, 'CS Elective', 3, '372'),
	(128, 1, 'CS Elective', 3, '411'),
	(129, 7, 'CS Elective', 3, '287'),
	(130, 7, 'CS Elective', 3, '387'),
	(131, 7, 'CS Elective', 3, '461'),
	(132, 1, 'CS Elective', 3, '340U'),
	(133, 1, 'CS Elective', 3, '480'),
	(134, 1, 'CS Elective', 3, '491'),
	(135, 3, 'Precalculus', 5, '125');
/*!40000 ALTER TABLE `Course` ENABLE KEYS */;

-- Dumping structure for table course_planner.CourseGroup
DROP TABLE IF EXISTS `CourseGroup`;
CREATE TABLE IF NOT EXISTS `CourseGroup` (
  `groupID` int NOT NULL AUTO_INCREMENT,
  `groupName` varchar(255) NOT NULL,
  PRIMARY KEY (`groupID`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table course_planner.CourseGroup: ~13 rows (approximately)
/*!40000 ALTER TABLE `CourseGroup` DISABLE KEYS */;
REPLACE INTO `CourseGroup` (`groupID`, `groupName`) VALUES
	(1, 'CSE Core'),
	(2, 'SE Core'),
	(3, 'CSE Electives'),
	(4, 'SE Electives'),
	(5, 'Foundation III - Global Perspectives'),
	(6, 'Foundation V - Mathematics, Formal Reasoning, Technology'),
	(7, 'Math/Stat Electives'),
	(8, 'Foundation I - English Composition'),
	(9, 'Foundation IV - Natural (Biological & Physical) Science'),
	(10, 'Foundation II - Creative Arts Humanities, Social Science'),
	(11, 'Experiential Learning'),
	(12, 'Computer Science'),
	(13, 'Advanced Writing Course'),
	(14, 'Software Engineering'),
	(15, 'Intercultural Perspectives'),
	(16, 'Not used');
/*!40000 ALTER TABLE `CourseGroup` ENABLE KEYS */;

-- Dumping structure for table course_planner.CourseGroupCourse
DROP TABLE IF EXISTS `CourseGroupCourse`;
CREATE TABLE IF NOT EXISTS `CourseGroupCourse` (
  `courseID` int NOT NULL,
  `groupID` int NOT NULL,
  PRIMARY KEY (`courseID`,`groupID`),
  KEY `groupID` (`groupID`),
  CONSTRAINT `CourseGroupCourse_ibfk_1` FOREIGN KEY (`courseID`) REFERENCES `Course` (`courseID`),
  CONSTRAINT `CourseGroupCourse_ibfk_2` FOREIGN KEY (`groupID`) REFERENCES `CourseGroup` (`groupID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table course_planner.CourseGroupCourse: ~131 rows (approximately)
/*!40000 ALTER TABLE `CourseGroupCourse` DISABLE KEYS */;
REPLACE INTO `CourseGroupCourse` (`courseID`, `groupID`) VALUES
	(1, 1),
	(2, 1),
	(3, 1),
	(6, 1),
	(8, 1),
	(9, 1),
	(10, 1),
	(11, 1),
	(13, 1),
	(15, 1),
	(20, 1),
	(68, 1),
	(69, 1),
	(71, 1),
	(77, 1),
	(78, 1),
	(81, 1),
	(135, 1),
	(1, 3),
	(4, 3),
	(5, 3),
	(7, 3),
	(12, 3),
	(70, 3),
	(72, 3),
	(73, 3),
	(74, 3),
	(75, 3),
	(76, 3),
	(79, 3),
	(80, 3),
	(82, 3),
	(83, 3),
	(84, 3),
	(114, 3),
	(115, 3),
	(116, 3),
	(117, 3),
	(118, 3),
	(119, 3),
	(120, 3),
	(121, 3),
	(122, 3),
	(123, 3),
	(124, 3),
	(125, 3),
	(126, 3),
	(127, 3),
	(128, 3),
	(129, 3),
	(130, 3),
	(131, 3),
	(132, 3),
	(133, 3),
	(134, 3),
	(103, 5),
	(13, 6),
	(14, 6),
	(15, 6),
	(16, 6),
	(17, 6),
	(18, 6),
	(19, 6),
	(20, 6),
	(13, 7),
	(14, 7),
	(15, 7),
	(16, 7),
	(17, 7),
	(18, 7),
	(19, 7),
	(20, 7),
	(89, 7),
	(90, 7),
	(91, 7),
	(92, 7),
	(93, 7),
	(94, 7),
	(95, 7),
	(96, 7),
	(97, 7),
	(98, 7),
	(104, 7),
	(105, 7),
	(106, 7),
	(107, 7),
	(108, 7),
	(109, 7),
	(110, 7),
	(111, 7),
	(112, 7),
	(113, 7),
	(135, 7),
	(26, 8),
	(30, 9),
	(31, 9),
	(33, 9),
	(34, 9),
	(38, 9),
	(39, 9),
	(40, 9),
	(41, 9),
	(43, 10),
	(44, 10),
	(46, 10),
	(102, 10),
	(77, 11),
	(78, 11),
	(13, 12),
	(14, 12),
	(15, 12),
	(16, 12),
	(17, 12),
	(18, 12),
	(19, 12),
	(27, 12),
	(28, 12),
	(29, 12),
	(30, 12),
	(31, 12),
	(33, 12),
	(34, 12),
	(38, 12),
	(39, 12),
	(88, 12),
	(85, 13),
	(99, 15),
	(86, 16),
	(87, 16),
	(99, 16),
	(100, 16),
	(101, 16);
/*!40000 ALTER TABLE `CourseGroupCourse` ENABLE KEYS */;

-- Dumping structure for table course_planner.CoursePreReq
DROP TABLE IF EXISTS `CoursePreReq`;
CREATE TABLE IF NOT EXISTS `CoursePreReq` (
  `courseID` int NOT NULL,
  `preReqID` varchar(40) NOT NULL,
  PRIMARY KEY (`courseID`,`preReqID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table course_planner.CoursePreReq: ~52 rows (approximately)
/*!40000 ALTER TABLE `CoursePreReq` DISABLE KEYS */;
REPLACE INTO `CoursePreReq` (`courseID`, `preReqID`) VALUES
	(3, '2'),
	(4, '15'),
	(4, '3'),
	(4, '9'),
	(5, '8'),
	(6, '26'),
	(8, '2'),
	(9, '8'),
	(10, '1'),
	(10, '8'),
	(13, '135'),
	(14, '13'),
	(15, '13'),
	(16, '13'),
	(17, '13'),
	(18, '13'),
	(19, '17,18'),
	(68, '15'),
	(68, '9'),
	(69, '10'),
	(70, '10'),
	(71, '10'),
	(72, '9'),
	(73, '13'),
	(73, '9,10'),
	(74, '73'),
	(75, '9'),
	(76, '69'),
	(77, '3'),
	(77, '9'),
	(78, '77'),
	(79, '71'),
	(80, '2'),
	(80, '34'),
	(81, '9'),
	(82, '71'),
	(83, '72'),
	(84, '15'),
	(84, '9'),
	(114, '9'),
	(116, '110'),
	(116, '2'),
	(117, '15'),
	(117, '9'),
	(118, '9'),
	(119, '68'),
	(120, '15,89'),
	(120, '9'),
	(121, '73'),
	(124, '1'),
	(125, '1'),
	(126, '1'),
	(128, '3'),
	(201, '2');
/*!40000 ALTER TABLE `CoursePreReq` ENABLE KEYS */;

-- Dumping structure for table course_planner.Degree
DROP TABLE IF EXISTS `Degree`;
CREATE TABLE IF NOT EXISTS `Degree` (
  `degreeID` int NOT NULL AUTO_INCREMENT,
  `degreeName` varchar(255) NOT NULL,
  PRIMARY KEY (`degreeID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table course_planner.Degree: ~2 rows (approximately)
/*!40000 ALTER TABLE `Degree` DISABLE KEYS */;
REPLACE INTO `Degree` (`degreeID`, `degreeName`) VALUES
	(1, 'CSE');
/*!40000 ALTER TABLE `Degree` ENABLE KEYS */;

-- Dumping structure for table course_planner.DegreePlan
DROP TABLE IF EXISTS `DegreePlan`;
CREATE TABLE IF NOT EXISTS `DegreePlan` (
  `planID` int NOT NULL,
  `degreeID` int NOT NULL,
  PRIMARY KEY (`planID`,`degreeID`),
  KEY `degreeID` (`degreeID`),
  CONSTRAINT `DegreePlan_ibfk_1` FOREIGN KEY (`degreeID`) REFERENCES `Degree` (`degreeID`),
  CONSTRAINT `DegreePlan_ibfk_2` FOREIGN KEY (`planID`) REFERENCES `Plan` (`planID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table course_planner.DegreePlan: ~25 rows (approximately)
/*!40000 ALTER TABLE `DegreePlan` DISABLE KEYS */;
REPLACE INTO `DegreePlan` (`planID`, `degreeID`) VALUES
	(32, 1),
	(108, 1),
	(112, 1),
	(114, 1),
	(115, 1),
	(126, 1),
	(127, 1),
	(128, 1),
	(129, 1),
	(130, 1),
	(131, 1),
	(132, 1),
	(133, 1),
	(134, 1),
	(135, 1),
	(136, 1),
	(138, 1),
	(139, 1),
	(141, 1),
	(145, 1),
	(146, 1),
	(156, 1),
	(157, 1),
	(164, 1),
	(169, 1),
	(172, 1);
/*!40000 ALTER TABLE `DegreePlan` ENABLE KEYS */;

-- Dumping structure for procedure course_planner.DeletePlan
DROP PROCEDURE IF EXISTS `DeletePlan`;
DELIMITER //
CREATE PROCEDURE `DeletePlan`(
	IN `PlanID` INT
)
    MODIFIES SQL DATA
BEGIN
DELETE FROM SemesterCourse s WHERE s.semesterID IN (SELECT semesterID FROM SemesterPlan p WHERE p.planID = planID);
DELETE FROM Plan p WHERE p.planID = PlanID;
DELETE FROM Comments c WHERE c.planID = PlanID;
DELETE FROM DegreePlan d WHERE d.planID = PlanID;
DELETE FROM SemesterPlan s WHERE s.planID = PlanID;
END//
DELIMITER ;

-- Dumping structure for procedure course_planner.DeleteSemesterCourse
DROP PROCEDURE IF EXISTS `DeleteSemesterCourse`;
DELIMITER //
CREATE PROCEDURE `DeleteSemesterCourse`(
	IN `semesterID` INT
)
BEGIN
DELETE FROM SemesterCourse s WHERE s.semesterID = semesterID;
END//
DELIMITER ;

-- Dumping structure for table course_planner.Department
DROP TABLE IF EXISTS `Department`;
CREATE TABLE IF NOT EXISTS `Department` (
  `departmentID` int NOT NULL AUTO_INCREMENT,
  `departmentAbbr` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`departmentID`)
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table course_planner.Department: ~27 rows (approximately)
/*!40000 ALTER TABLE `Department` DISABLE KEYS */;
REPLACE INTO `Department` (`departmentID`, `departmentAbbr`) VALUES
	(1, 'CSE'),
	(2, 'CEC'),
	(3, 'MTH'),
	(4, 'STA'),
	(5, 'IMS'),
	(6, 'PHY'),
	(7, 'ECE'),
	(8, 'ISA'),
	(9, 'ENG'),
	(10, 'ECO'),
	(11, 'AAA'),
	(12, 'CHM'),
	(13, 'ARC'),
	(14, 'CLS'),
	(15, 'FRE'),
	(16, 'HST'),
	(17, 'ART'),
	(18, 'EDP'),
	(19, 'BIO'),
	(20, 'BIO/MBI'),
	(21, 'STC'),
	(22, 'GEO'),
	(23, 'CHI'),
	(24, 'Global'),
	(25, 'Creative'),
	(26, 'Intercultural'),
	(27, 'IP'),
	(28, 'CA'),
	(29, 'GL');
/*!40000 ALTER TABLE `Department` ENABLE KEYS */;

-- Dumping structure for procedure course_planner.GetAllCourseDetails
DROP PROCEDURE IF EXISTS `GetAllCourseDetails`;
DELIMITER //
CREATE PROCEDURE `GetAllCourseDetails`()
BEGIN
SELECT Course.courseID as CourseID, courseName as CourseName, CourseGroup.groupID as GroupID, groupName as CourseGroup, Course.departmentID as DepartmentID, departmentAbbr as DepartmentName, Course.courseDescription as Description, creditHours as Credits FROM Course JOIN CourseGroupCourse ON Course.courseID = CourseGroupCourse.courseID JOIN CourseGroup ON CourseGroupCourse.groupID = CourseGroup.groupID JOIN Department ON Course.departmentID = Department.departmentID;
END//
DELIMITER ;

-- Dumping structure for procedure course_planner.GetAllCourses
DROP PROCEDURE IF EXISTS `GetAllCourses`;
DELIMITER //
CREATE PROCEDURE `GetAllCourses`()
BEGIN
SELECT * FROM Course;
END//
DELIMITER ;

-- Dumping structure for procedure course_planner.GetAllPreReq
DROP PROCEDURE IF EXISTS `GetAllPreReq`;
DELIMITER //
CREATE PROCEDURE `GetAllPreReq`(
	IN `courseID` INT
)
BEGIN
SELECT preReqID FROM CoursePreReq c WHERE c.courseID = courseID;
END//
DELIMITER ;

-- Dumping structure for procedure course_planner.GetComments
DROP PROCEDURE IF EXISTS `GetComments`;
DELIMITER //
CREATE PROCEDURE `GetComments`(
	IN `planID` INT
)
BEGIN
SELECT c.Text,
	DATE_FORMAT(Date, '%a %M %e %Y %h:%i %p') as DATE, p.miamiID FROM Comments c JOIN Person p ON c.PersonID = p.personID WHERE c.PlanID = planID ORDER BY 2;
END//
DELIMITER ;

-- Dumping structure for procedure course_planner.GetCourseGroups
DROP PROCEDURE IF EXISTS `GetCourseGroups`;
DELIMITER //
CREATE PROCEDURE `GetCourseGroups`(
	IN `degreeID` INT,
	IN `reqYear` INT
)
BEGIN
SELECT * FROM CourseGroup JOIN RequirementCourseGroup ON CourseGroup.groupID = RequirementCourseGroup.groupID WHERE RequirementCourseGroup.degreeID = degreeID AND RequirementCourseGroup.reqYear = reqYear;
END//
DELIMITER ;

-- Dumping structure for procedure course_planner.GetCoursesForGroup
DROP PROCEDURE IF EXISTS `GetCoursesForGroup`;
DELIMITER //
CREATE PROCEDURE `GetCoursesForGroup`(
	IN `groupID` INT
)
BEGIN
SELECT cgc.courseID, c.courseName, cgc.groupID, cg.groupName, d.departmentID, d.departmentAbbr as departmentName, c.courseDescription, c.creditHours as Credits FROM CourseGroupCourse cgc JOIN Course c ON cgc.courseID = c.courseID JOIN CourseGroup cg ON cgc.groupID = cg.groupID JOIN Department d ON c.departmentID = d.departmentID WHERE cgc.groupID = groupID ORDER BY d.departmentAbbr, c.courseName;
END//
DELIMITER ;

-- Dumping structure for procedure course_planner.GetDegreeID
DROP PROCEDURE IF EXISTS `GetDegreeID`;
DELIMITER //
CREATE PROCEDURE `GetDegreeID`(
	IN `planID` INT
)
BEGIN
SELECT dp.degreeID FROM Plan p JOIN DegreePlan dp ON p.planID = dp.planID WHERE p.planID = planID;
END//
DELIMITER ;

-- Dumping structure for procedure course_planner.GetDetailedPlans
DROP PROCEDURE IF EXISTS `GetDetailedPlans`;
DELIMITER //
CREATE PROCEDURE `GetDetailedPlans`(
	IN `planID` INT
)
BEGIN
SELECT sp.planID, sp.semesterID, sp.season, sp.currentYear, c.*, d.departmentAbbr FROM SemesterPlan sp LEFT JOIN SemesterCourse sc ON sp.semesterID = sc.semesterID LEFT JOIN Course c ON sc.courseID = c.courseID LEFT JOIN Department d ON c.departmentID = d.departmentID WHERE sp.planID = planID;
END//
DELIMITER ;

-- Dumping structure for procedure course_planner.GetNumOwnedPlans
DROP PROCEDURE IF EXISTS `GetNumOwnedPlans`;
DELIMITER //
CREATE PROCEDURE `GetNumOwnedPlans`(
	IN `planID` TEXT,
	IN `personID` TEXT
)
    READS SQL DATA
BEGIN
SELECT COUNT(*) FROM `Plan` p LEFT JOIN `StudentAdvisor` st ON st.studentPersonID = p.personID WHERE p.planID = planID AND (p.personID = personID OR st.advisorPersonID = personID);
END//
DELIMITER ;

-- Dumping structure for procedure course_planner.GetPerson
DROP PROCEDURE IF EXISTS `GetPerson`;
DELIMITER //
CREATE PROCEDURE `GetPerson`(
	IN `miamiID` VARCHAR(50)
)
BEGIN
SELECT * FROM Person p WHERE p.miamiID = miamiID;
END//
DELIMITER ;

-- Dumping structure for procedure course_planner.GetPlanDetails
DROP PROCEDURE IF EXISTS `GetPlanDetails`;
DELIMITER //
CREATE PROCEDURE `GetPlanDetails`(
	IN `miamiID` VARCHAR(50),
	IN `personID` INT
)
BEGIN
(SELECT Plan.planID as planID, planName, enrollYear, enrollSeason, COUNT(DISTINCT currentYear) as planLength, degreeName, Person.miamiID
	                                FROM Plan
	                                JOIN Person ON Person.personID = Plan.personID
	                                JOIN SemesterPlan ON SemesterPlan.planID = Plan.planID
	                                JOIN DegreePlan ON DegreePlan.planID = Plan.planID
	                                JOIN Degree ON Degree.degreeID = DegreePlan.degreeID
	                                WHERE Person.miamiID = miamiID
	                                GROUP BY planID, planName, enrollYear, enrollSeason, degreeName)
                                UNION
                                (SELECT Plan.planID as planID, planName, enrollYear, enrollSeason, COUNT(DISTINCT currentYear) as planLength, degreeName, Person.miamiID
	                                FROM Plan
	                                JOIN Person ON Person.personID = Plan.personID
	                                JOIN SemesterPlan ON SemesterPlan.planID = Plan.planID
	                                JOIN DegreePlan ON DegreePlan.planID = Plan.planID
	                                JOIN Degree ON Degree.degreeID = DegreePlan.degreeID
                                    JOIN StudentAdvisor st ON st.studentPersonID = Plan.personID
                                    WHERE st.advisorPersonID = personID
	                                GROUP BY planID, planName, enrollYear, enrollSeason, degreeName);
END//
DELIMITER ;

-- Dumping structure for procedure course_planner.GetPlanYear
DROP PROCEDURE IF EXISTS `GetPlanYear`;
DELIMITER //
CREATE PROCEDURE `GetPlanYear`(
	IN `planID` INT
)
BEGIN
SELECT CONCAT(p.enrollYear, p.enrollYear % 100 + 1) FROM Plan p WHERE p.planID = planID;
END//
DELIMITER ;

-- Dumping structure for procedure course_planner.GetSemesterID
DROP PROCEDURE IF EXISTS `GetSemesterID`;
DELIMITER //
CREATE PROCEDURE `GetSemesterID`(
	IN `planID` INT
)
BEGIN
SELECT semesterID FROM SemesterPlan s WHERE s.planID = planID;
END//
DELIMITER ;

-- Dumping structure for procedure course_planner.InsertNewDegreePlan
DROP PROCEDURE IF EXISTS `InsertNewDegreePlan`;
DELIMITER //
CREATE PROCEDURE `InsertNewDegreePlan`(
	IN `PlanID` INT,
	IN `DegreeID` INT
)
BEGIN
INSERT INTO DegreePlan (planID, degreeID) VALUES (PlanID, DegreeID);
END//
DELIMITER ;

-- Dumping structure for procedure course_planner.InsertNewPlan
DROP PROCEDURE IF EXISTS `InsertNewPlan`;
DELIMITER //
CREATE PROCEDURE `InsertNewPlan`(
	IN `PersonID` INT,
	IN `PlanName` VARCHAR(50),
	IN `EnrollYear` INT,
	IN `EnrollSeason` VARCHAR(50)
)
    MODIFIES SQL DATA
BEGIN
INSERT INTO Plan (personID, planName, enrollYear, enrollSeason) VALUES (PersonID, PlanName, EnrollYear, EnrollSeason);
SELECT last_insert_id();
END//
DELIMITER ;

-- Dumping structure for procedure course_planner.InsertNewSemesterPlan
DROP PROCEDURE IF EXISTS `InsertNewSemesterPlan`;
DELIMITER //
CREATE PROCEDURE `InsertNewSemesterPlan`(
	IN `PlanID` INT,
	IN `Season` VARCHAR(50),
	IN `CurrentYear` INT
)
    MODIFIES SQL DATA
BEGIN
INSERT INTO SemesterPlan (planID, season, currentYear) VALUES (PlanID, Season, CurrentYear);
END//
DELIMITER ;

-- Dumping structure for procedure course_planner.InsertPerson
DROP PROCEDURE IF EXISTS `InsertPerson`;
DELIMITER //
CREATE PROCEDURE `InsertPerson`(
	IN `miamiID` VARCHAR(50)
)
BEGIN
INSERT INTO Person (miamiID, isAdmin, isAdvisor) VALUES (miamiID, 0, 0);
END//
DELIMITER ;

-- Dumping structure for procedure course_planner.InsertSemesterCourse
DROP PROCEDURE IF EXISTS `InsertSemesterCourse`;
DELIMITER //
CREATE PROCEDURE `InsertSemesterCourse`(
	IN `semesterID` INT,
	IN `courseID` INT
)
BEGIN
INSERT INTO SemesterCourse (semesterID, courseID) VALUES (semesterID, courseID);
END//
DELIMITER ;

-- Dumping structure for table course_planner.Person
DROP TABLE IF EXISTS `Person`;
CREATE TABLE IF NOT EXISTS `Person` (
  `personID` int NOT NULL AUTO_INCREMENT,
  `miamiID` varchar(50) NOT NULL,
  `isAdmin` tinyint(1) NOT NULL,
  `isAdvisor` tinyint(1) NOT NULL,
  PRIMARY KEY (`personID`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table course_planner.Person: ~12 rows (approximately)
/*!40000 ALTER TABLE `Person` DISABLE KEYS */;
REPLACE INTO `Person` (`personID`, `miamiID`, `isAdmin`, `isAdvisor`) VALUES
	(1, 'xiex8', 0, 0),
	(5, 'rizkalle', 0, 0),
	(6, 'wangj208', 0, 0),
	(7, 'maix', 0, 0),
	(9, 'wiedemwm', 0, 0),
	(11, 'stahrm', 0, 0),
	(15, 'hilgerbj', 0, 0),
	(16, 'stahrlc', 0, 0),
	(17, 'rajwanss', 0, 0),
	(18, 'perryna4', 1, 1),
	(19, 'alberstr', 0, 0),
	(20, 'crailjc', 1, 1),
	(21, 'baileyd7', 0, 0);
/*!40000 ALTER TABLE `Person` ENABLE KEYS */;

-- Dumping structure for table course_planner.Plan
DROP TABLE IF EXISTS `Plan`;
CREATE TABLE IF NOT EXISTS `Plan` (
  `planID` int NOT NULL AUTO_INCREMENT,
  `planName` varchar(255) NOT NULL,
  `personID` int NOT NULL,
  `enrollYear` int NOT NULL,
  `enrollSeason` varchar(10) NOT NULL,
  PRIMARY KEY (`planID`),
  KEY `fk_personID` (`personID`),
  CONSTRAINT `fk_personID` FOREIGN KEY (`personID`) REFERENCES `Person` (`personID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=174 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table course_planner.Plan: ~34 rows (approximately)
/*!40000 ALTER TABLE `Plan` DISABLE KEYS */;
REPLACE INTO `Plan` (`planID`, `planName`, `personID`, `enrollYear`, `enrollSeason`) VALUES
	(20, 'Test', 1, 2021, 'Fall'),
	(27, 'InsertTest', 1, 2022, 'Spring'),
	(28, 'InsertTest', 1, 2022, 'Spring'),
	(29, 'InsertTest', 1, 2022, 'Spring'),
	(30, 'TTTest', 1, 2019, 'Spring'),
	(31, 'TTTest', 1, 2019, 'Spring'),
	(32, 'TTTest', 1, 2019, 'Spring'),
	(89, 'lcs name', 1, 2021, 'Fall'),
	(97, 'Test', 1, 2021, 'Fall'),
	(100, 'My Awesome Plan', 1, 2021, 'Fall'),
	(108, 'Xinrui\'s Test', 1, 2014, 'Fall'),
	(112, 'ww', 7, 2021, 'Fall'),
	(114, 'rizkalle', 5, 2021, 'Fall'),
	(115, 'rizkalle_3', 5, 2021, 'Fall'),
	(126, 'Plan 1', 9, 2021, 'Fall'),
	(127, 'Plan 2', 9, 2021, 'Fall'),
	(128, 'NextPlan', 9, 2021, 'Fall'),
	(129, 'Plan15', 9, 2018, 'Fall'),
	(130, 'my plan to be famous', 16, 2021, 'Fall'),
	(131, 'another plan to be famous', 16, 2021, 'Fall'),
	(132, 'Test', 15, 2021, 'Fall'),
	(133, 'study abroad', 5, 2021, 'Fall'),
	(134, 'Test', 5, 2021, 'Fall'),
	(135, 'new', 9, 2021, 'Fall'),
	(136, 'urdiy', 17, 2022, 'Fall'),
	(138, 'Stahr', 11, 2022, 'Fall'),
	(139, 'Tyler Albers Test Plan', 19, 2019, 'Fall'),
	(141, 'Test2', 20, 2022, 'Fall'),
	(145, 'Another test-DB', 21, 2020, 'Fall'),
	(146, 'Works?', 20, 2021, 'Fall'),
	(156, 'NewView', 20, 2022, 'Fall'),
	(157, 'NewerView', 20, 2022, 'Fall'),
	(164, 'del4', 21, 2022, 'Fall'),
	(169, 'Test', 20, 2022, 'Fall'),
	(172, 'TREST', 18, 2022, 'Fall');
/*!40000 ALTER TABLE `Plan` ENABLE KEYS */;

-- Dumping structure for procedure course_planner.PlanContainsSemester
DROP PROCEDURE IF EXISTS `PlanContainsSemester`;
DELIMITER //
CREATE PROCEDURE `PlanContainsSemester`(
	IN `planID` INT,
	IN `season` VARCHAR(50),
	IN `currentYear` INT
)
BEGIN
SELECT COUNT(*) FROM SemesterPlan s WHERE s.planID = planID AND s.season = season AND s.currentYear = currentYear;
END//
DELIMITER ;

-- Dumping structure for procedure course_planner.RemoveCourseFromSemester
DROP PROCEDURE IF EXISTS `RemoveCourseFromSemester`;
DELIMITER //
CREATE PROCEDURE `RemoveCourseFromSemester`(
	IN `courseID` INT,
	IN `semesterID` INT
)
BEGIN
DELETE FROM SemesterCourse s WHERE s.courseID = courseID AND s.semesterID = semesterID;
END//
DELIMITER ;

-- Dumping structure for procedure course_planner.RemoveEquivalentCourse
DROP PROCEDURE IF EXISTS `RemoveEquivalentCourse`;
DELIMITER //
CREATE PROCEDURE `RemoveEquivalentCourse`(
	IN `courseID` INT,
	IN `equivCourse` INT
)
BEGIN
DELETE FROM EquivalentCourse WHERE e.courseID = courseID AND e.equivCourseID = equivCourse;
END//
DELIMITER ;

-- Dumping structure for procedure course_planner.RemoveSemesterFromPlan
DROP PROCEDURE IF EXISTS `RemoveSemesterFromPlan`;
DELIMITER //
CREATE PROCEDURE `RemoveSemesterFromPlan`(
	IN `semesterID` INT,
	IN `planID` INT
)
BEGIN
DELETE FROM SemesterPlan s WHERE s.semesterID = semesterID AND s.planID = planID;
END//
DELIMITER ;

-- Dumping structure for procedure course_planner.RemoveSemesters
DROP PROCEDURE IF EXISTS `RemoveSemesters`;
DELIMITER //
CREATE PROCEDURE `RemoveSemesters`(
	IN `semesterID` INT,
	IN `planID` INT
)
BEGIN
DELETE FROM SemesterCourse s WHERE s.semesterID = semesterID;
DELETE FROM SemesterPlan p WHERE p.planID = planID AND p.semesterID = semesterID;
END//
DELIMITER ;

-- Dumping structure for table course_planner.RequirementCourseGroup
DROP TABLE IF EXISTS `RequirementCourseGroup`;
CREATE TABLE IF NOT EXISTS `RequirementCourseGroup` (
  `degreeID` int NOT NULL,
  `groupID` int NOT NULL,
  `reqYear` int NOT NULL,
  `minCreditHours` int DEFAULT NULL,
  `maxCreditHourse` int DEFAULT NULL,
  PRIMARY KEY (`degreeID`,`groupID`,`reqYear`),
  KEY `groupID` (`groupID`),
  CONSTRAINT `RequirementCourseGroup_ibfk_1` FOREIGN KEY (`groupID`) REFERENCES `CourseGroup` (`groupID`),
  CONSTRAINT `RequirementCourseGroup_ibfk_2` FOREIGN KEY (`degreeID`) REFERENCES `Degree` (`degreeID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table course_planner.RequirementCourseGroup: ~105 rows (approximately)
/*!40000 ALTER TABLE `RequirementCourseGroup` DISABLE KEYS */;
REPLACE INTO `RequirementCourseGroup` (`degreeID`, `groupID`, `reqYear`, `minCreditHours`, `maxCreditHourse`) VALUES
	(1, 1, 201314, NULL, NULL),
	(1, 1, 201415, NULL, NULL),
	(1, 1, 201516, NULL, NULL),
	(1, 1, 201617, NULL, NULL),
	(1, 1, 201718, NULL, NULL),
	(1, 1, 201819, NULL, NULL),
	(1, 1, 201920, NULL, NULL),
	(1, 1, 202021, NULL, NULL),
	(1, 1, 202122, NULL, NULL),
	(1, 1, 202223, NULL, NULL),
	(1, 3, 201314, NULL, NULL),
	(1, 3, 201415, NULL, NULL),
	(1, 3, 201516, NULL, NULL),
	(1, 3, 201617, NULL, NULL),
	(1, 3, 201718, NULL, NULL),
	(1, 3, 201819, NULL, NULL),
	(1, 3, 201920, NULL, NULL),
	(1, 3, 202021, NULL, NULL),
	(1, 3, 202122, NULL, NULL),
	(1, 3, 202223, NULL, NULL),
	(1, 5, 201314, NULL, NULL),
	(1, 5, 201415, NULL, NULL),
	(1, 5, 201516, NULL, NULL),
	(1, 5, 201617, NULL, NULL),
	(1, 5, 201718, NULL, NULL),
	(1, 5, 201819, NULL, NULL),
	(1, 5, 201920, NULL, NULL),
	(1, 5, 202021, NULL, NULL),
	(1, 5, 202122, NULL, NULL),
	(1, 5, 202223, NULL, NULL),
	(1, 6, 201314, NULL, NULL),
	(1, 6, 201415, NULL, NULL),
	(1, 6, 201516, NULL, NULL),
	(1, 6, 201617, NULL, NULL),
	(1, 6, 201718, NULL, NULL),
	(1, 6, 201819, NULL, NULL),
	(1, 6, 201920, NULL, NULL),
	(1, 6, 202021, NULL, NULL),
	(1, 6, 202122, NULL, NULL),
	(1, 6, 202223, NULL, NULL),
	(1, 7, 201314, NULL, NULL),
	(1, 7, 201415, NULL, NULL),
	(1, 7, 201516, NULL, NULL),
	(1, 7, 201617, NULL, NULL),
	(1, 7, 201718, NULL, NULL),
	(1, 7, 201819, NULL, NULL),
	(1, 7, 201920, NULL, NULL),
	(1, 7, 202021, NULL, NULL),
	(1, 7, 202122, NULL, NULL),
	(1, 7, 202223, NULL, NULL),
	(1, 8, 201314, NULL, NULL),
	(1, 8, 201415, NULL, NULL),
	(1, 8, 201516, NULL, NULL),
	(1, 8, 201617, NULL, NULL),
	(1, 8, 201718, NULL, NULL),
	(1, 8, 201819, NULL, NULL),
	(1, 8, 201920, NULL, NULL),
	(1, 8, 202021, NULL, NULL),
	(1, 8, 202122, NULL, NULL),
	(1, 8, 202223, NULL, NULL),
	(1, 9, 201314, NULL, NULL),
	(1, 9, 201415, NULL, NULL),
	(1, 9, 201516, NULL, NULL),
	(1, 9, 201617, NULL, NULL),
	(1, 9, 201718, NULL, NULL),
	(1, 9, 201819, NULL, NULL),
	(1, 9, 201920, NULL, NULL),
	(1, 9, 202021, NULL, NULL),
	(1, 9, 202122, NULL, NULL),
	(1, 9, 202223, NULL, NULL),
	(1, 10, 201314, NULL, NULL),
	(1, 10, 201415, NULL, NULL),
	(1, 10, 201516, NULL, NULL),
	(1, 10, 201617, NULL, NULL),
	(1, 10, 201718, NULL, NULL),
	(1, 10, 201819, NULL, NULL),
	(1, 10, 201920, NULL, NULL),
	(1, 10, 202021, NULL, NULL),
	(1, 10, 202122, NULL, NULL),
	(1, 10, 202223, NULL, NULL),
	(1, 11, 201314, NULL, NULL),
	(1, 11, 201415, NULL, NULL),
	(1, 11, 201516, NULL, NULL),
	(1, 11, 201617, NULL, NULL),
	(1, 11, 201718, NULL, NULL),
	(1, 11, 201819, NULL, NULL),
	(1, 11, 201920, NULL, NULL),
	(1, 11, 202021, NULL, NULL),
	(1, 11, 202122, NULL, NULL),
	(1, 11, 202223, NULL, NULL),
	(1, 13, 201314, NULL, NULL),
	(1, 13, 201415, NULL, NULL),
	(1, 13, 201516, NULL, NULL),
	(1, 13, 201617, NULL, NULL),
	(1, 13, 201718, NULL, NULL),
	(1, 13, 201819, NULL, NULL),
	(1, 13, 201920, NULL, NULL),
	(1, 13, 202021, NULL, NULL),
	(1, 13, 202122, NULL, NULL),
	(1, 13, 202223, NULL, NULL),
	(1, 15, 201314, NULL, NULL),
	(1, 15, 201415, NULL, NULL),
	(1, 15, 201516, NULL, NULL),
	(1, 15, 201617, NULL, NULL),
	(1, 15, 201718, NULL, NULL),
	(1, 15, 201819, NULL, NULL),
	(1, 15, 201920, NULL, NULL),
	(1, 15, 202021, NULL, NULL),
	(1, 15, 202122, NULL, NULL),
	(1, 15, 202223, NULL, NULL);
/*!40000 ALTER TABLE `RequirementCourseGroup` ENABLE KEYS */;

-- Dumping structure for procedure course_planner.SelectAllPlans
DROP PROCEDURE IF EXISTS `SelectAllPlans`;
DELIMITER //
CREATE PROCEDURE `SelectAllPlans`(
	IN `personID` INT
)
    READS SQL DATA
BEGIN
SELECT * FROM Plan p WHERE p.personID = personID;
END//
DELIMITER ;

-- Dumping structure for procedure course_planner.SemesterContainsCourse
DROP PROCEDURE IF EXISTS `SemesterContainsCourse`;
DELIMITER //
CREATE PROCEDURE `SemesterContainsCourse`(
	IN `semesterID` INT,
	IN `courseID` INT
)
BEGIN
SELECT COUNT(*) FROM SemesterCourse s WHERE s.semesterID = semesterID AND s.courseID = courseID;
END//
DELIMITER ;

-- Dumping structure for table course_planner.SemesterCourse
DROP TABLE IF EXISTS `SemesterCourse`;
CREATE TABLE IF NOT EXISTS `SemesterCourse` (
  `courseID` int NOT NULL,
  `semesterID` int NOT NULL,
  PRIMARY KEY (`courseID`,`semesterID`),
  KEY `fk_semesterID_idx` (`semesterID`),
  CONSTRAINT `fk_courseID` FOREIGN KEY (`courseID`) REFERENCES `Course` (`courseID`),
  CONSTRAINT `fk_semesterID` FOREIGN KEY (`semesterID`) REFERENCES `SemesterPlan` (`semesterID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table course_planner.SemesterCourse: ~74 rows (approximately)
/*!40000 ALTER TABLE `SemesterCourse` DISABLE KEYS */;
REPLACE INTO `SemesterCourse` (`courseID`, `semesterID`) VALUES
	(1, 103),
	(2, 103),
	(3, 670),
	(4, 670),
	(5, 670),
	(6, 671),
	(7, 671),
	(8, 672),
	(9, 672),
	(2, 950),
	(3, 950),
	(11, 1026),
	(1, 1052),
	(2, 1054),
	(1, 1084),
	(11, 1084),
	(15, 1084),
	(20, 1084),
	(2, 1085),
	(13, 1085),
	(11, 1325),
	(2, 1327),
	(46, 1327),
	(135, 1345),
	(135, 1350),
	(11, 1382),
	(20, 1383),
	(13, 1394),
	(135, 1394),
	(135, 1417),
	(2, 1418),
	(11, 1433),
	(26, 1451),
	(13, 1461),
	(2, 1462),
	(17, 1463),
	(18, 1464),
	(19, 1464),
	(11, 1477),
	(3, 1718),
	(11, 1718),
	(11, 1959),
	(69, 1959),
	(1, 1960),
	(8, 1961),
	(9, 1962),
	(2, 1975),
	(3, 1975),
	(6, 1975),
	(10, 1975),
	(68, 1975),
	(2, 1976),
	(19, 1977),
	(11, 1980),
	(26, 1981),
	(3, 1982),
	(1, 1992),
	(6, 1992),
	(16, 1992),
	(17, 1992),
	(11, 2111),
	(1, 2177),
	(6, 2177),
	(16, 2177),
	(17, 2177),
	(1, 2192),
	(6, 2192),
	(16, 2192),
	(17, 2192),
	(3, 2210),
	(1, 2226),
	(2, 2226),
	(6, 2226),
	(11, 2226),
	(1, 2231),
	(2, 2231),
	(6, 2231),
	(8, 2231),
	(11, 2231),
	(1, 2282),
	(2, 2282),
	(3, 2282),
	(6, 2282),
	(8, 2282),
	(9, 2282),
	(10, 2282);
/*!40000 ALTER TABLE `SemesterCourse` ENABLE KEYS */;

-- Dumping structure for table course_planner.SemesterPlan
DROP TABLE IF EXISTS `SemesterPlan`;
CREATE TABLE IF NOT EXISTS `SemesterPlan` (
  `semesterID` int NOT NULL AUTO_INCREMENT,
  `planID` int NOT NULL,
  `season` varchar(100) NOT NULL,
  `currentYear` int NOT NULL,
  PRIMARY KEY (`semesterID`,`planID`),
  KEY `fk_planID` (`planID`),
  CONSTRAINT `fk_planID` FOREIGN KEY (`planID`) REFERENCES `Plan` (`planID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2300 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table course_planner.SemesterPlan: ~428 rows (approximately)
/*!40000 ALTER TABLE `SemesterPlan` DISABLE KEYS */;
REPLACE INTO `SemesterPlan` (`semesterID`, `planID`, `season`, `currentYear`) VALUES
	(103, 20, 'Fall', 202122),
	(666, 89, 'Fall', 202122),
	(667, 89, 'Spring', 202122),
	(668, 89, 'Fall', 202223),
	(669, 89, 'Spring', 202223),
	(670, 89, 'Fall', 202324),
	(671, 89, 'Spring', 202324),
	(672, 89, 'Fall', 202425),
	(673, 89, 'Spring', 202425),
	(950, 108, 'Fall', 201415),
	(951, 108, 'Winter', 201415),
	(952, 108, 'Spring', 201415),
	(953, 108, 'Summer', 201415),
	(954, 108, 'Fall', 201516),
	(955, 108, 'Winter', 201516),
	(956, 108, 'Spring', 201516),
	(957, 108, 'Summer', 201516),
	(958, 108, 'Fall', 201617),
	(959, 108, 'Winter', 201617),
	(960, 108, 'Spring', 201617),
	(961, 108, 'Summer', 201617),
	(962, 108, 'Fall', 201718),
	(963, 108, 'Winter', 201718),
	(964, 108, 'Spring', 201718),
	(965, 108, 'Summer', 201718),
	(966, 108, 'Completed', 201415),
	(1026, 112, 'Fall', 202324),
	(1027, 112, 'Winter', 202324),
	(1028, 112, 'Spring', 202324),
	(1029, 112, 'Summer', 202324),
	(1030, 112, 'Fall', 202425),
	(1031, 112, 'Winter', 202425),
	(1032, 112, 'Spring', 202425),
	(1033, 112, 'Summer', 202425),
	(1034, 112, 'Completed', 202122),
	(1052, 114, 'Fall', 202122),
	(1053, 114, 'Winter', 202122),
	(1054, 114, 'Spring', 202122),
	(1055, 114, 'Summer', 202122),
	(1056, 114, 'Fall', 202223),
	(1057, 114, 'Winter', 202223),
	(1058, 114, 'Spring', 202223),
	(1059, 114, 'Summer', 202223),
	(1060, 114, 'Fall', 202324),
	(1061, 114, 'Winter', 202324),
	(1062, 114, 'Spring', 202324),
	(1063, 114, 'Summer', 202324),
	(1064, 114, 'Fall', 202425),
	(1065, 114, 'Winter', 202425),
	(1066, 114, 'Spring', 202425),
	(1067, 114, 'Summer', 202425),
	(1068, 114, 'Completed', 202122),
	(1084, 115, 'Fall', 202122),
	(1085, 115, 'Winter', 202122),
	(1086, 115, 'Spring', 202122),
	(1087, 115, 'Summer', 202122),
	(1088, 115, 'Fall', 202223),
	(1089, 115, 'Winter', 202223),
	(1090, 115, 'Spring', 202223),
	(1091, 115, 'Summer', 202223),
	(1092, 115, 'Fall', 202324),
	(1093, 115, 'Winter', 202324),
	(1094, 115, 'Spring', 202324),
	(1095, 115, 'Summer', 202324),
	(1096, 115, 'Fall', 202425),
	(1097, 115, 'Winter', 202425),
	(1098, 115, 'Spring', 202425),
	(1099, 115, 'Summer', 202425),
	(1100, 115, 'Completed', 202122),
	(1101, 115, 'Fall', 202526),
	(1102, 115, 'Winter', 202526),
	(1103, 115, 'Spring', 202526),
	(1104, 115, 'Summer', 202526),
	(1105, 115, 'Completed', 202526),
	(1179, 115, 'Fall', 202627),
	(1180, 115, 'Winter', 202627),
	(1181, 115, 'Spring', 202627),
	(1182, 115, 'Summer', 202627),
	(1183, 115, 'Completed', 202627),
	(1184, 115, 'Fall', 202728),
	(1185, 115, 'Winter', 202728),
	(1186, 115, 'Spring', 202728),
	(1187, 115, 'Summer', 202728),
	(1188, 115, 'Completed', 202728),
	(1189, 115, 'Fall', 202829),
	(1190, 115, 'Winter', 202829),
	(1191, 115, 'Spring', 202829),
	(1192, 115, 'Summer', 202829),
	(1193, 115, 'Completed', 202829),
	(1325, 126, 'Fall', 202122),
	(1326, 126, 'Winter', 202122),
	(1327, 126, 'Spring', 202122),
	(1328, 126, 'Summer', 202122),
	(1329, 126, 'Fall', 202223),
	(1330, 126, 'Winter', 202223),
	(1331, 126, 'Spring', 202223),
	(1332, 126, 'Summer', 202223),
	(1337, 126, 'Fall', 202425),
	(1338, 126, 'Winter', 202425),
	(1339, 126, 'Spring', 202425),
	(1340, 126, 'Summer', 202425),
	(1341, 126, 'Fall', 202526),
	(1342, 126, 'Winter', 202526),
	(1343, 126, 'Spring', 202526),
	(1344, 126, 'Summer', 202526),
	(1345, 126, 'Completed', 202122),
	(1346, 126, 'Fall', 202627),
	(1347, 126, 'Winter', 202627),
	(1348, 126, 'Spring', 202627),
	(1349, 126, 'Summer', 202627),
	(1350, 126, 'Completed', 202627),
	(1351, 127, 'Fall', 202122),
	(1352, 127, 'Winter', 202122),
	(1353, 127, 'Spring', 202122),
	(1354, 127, 'Summer', 202122),
	(1355, 127, 'Fall', 202223),
	(1356, 127, 'Winter', 202223),
	(1357, 127, 'Spring', 202223),
	(1358, 127, 'Summer', 202223),
	(1359, 127, 'Fall', 202324),
	(1360, 127, 'Winter', 202324),
	(1361, 127, 'Spring', 202324),
	(1362, 127, 'Summer', 202324),
	(1363, 127, 'Fall', 202425),
	(1364, 127, 'Winter', 202425),
	(1365, 127, 'Spring', 202425),
	(1366, 127, 'Summer', 202425),
	(1367, 127, 'Completed', 202122),
	(1368, 112, 'Fall', 202526),
	(1369, 112, 'Winter', 202526),
	(1370, 112, 'Spring', 202526),
	(1371, 112, 'Summer', 202526),
	(1372, 112, 'Completed', 202526),
	(1377, 112, 'Completed', 202627),
	(1382, 128, 'Fall', 202223),
	(1383, 128, 'Winter', 202223),
	(1384, 128, 'Spring', 202223),
	(1385, 128, 'Summer', 202223),
	(1386, 128, 'Fall', 202324),
	(1387, 128, 'Winter', 202324),
	(1388, 128, 'Spring', 202324),
	(1389, 128, 'Summer', 202324),
	(1390, 128, 'Fall', 202425),
	(1391, 128, 'Winter', 202425),
	(1392, 128, 'Spring', 202425),
	(1393, 128, 'Summer', 202425),
	(1394, 128, 'Completed', 202122),
	(1395, 128, 'Fall', 202526),
	(1396, 128, 'Winter', 202526),
	(1397, 128, 'Spring', 202526),
	(1398, 128, 'Summer', 202526),
	(1399, 128, 'Completed', 202526),
	(1400, 129, 'Fall', 201819),
	(1401, 129, 'Winter', 201819),
	(1402, 129, 'Spring', 201819),
	(1403, 129, 'Summer', 201819),
	(1404, 129, 'Fall', 201920),
	(1405, 129, 'Winter', 201920),
	(1406, 129, 'Spring', 201920),
	(1407, 129, 'Summer', 201920),
	(1408, 129, 'Fall', 202021),
	(1409, 129, 'Winter', 202021),
	(1410, 129, 'Spring', 202021),
	(1411, 129, 'Summer', 202021),
	(1412, 129, 'Fall', 202122),
	(1413, 129, 'Winter', 202122),
	(1414, 129, 'Spring', 202122),
	(1415, 129, 'Summer', 202122),
	(1416, 129, 'Completed', 201819),
	(1417, 130, 'Fall', 202122),
	(1418, 130, 'Winter', 202122),
	(1419, 130, 'Spring', 202122),
	(1420, 130, 'Summer', 202122),
	(1421, 130, 'Fall', 202223),
	(1422, 130, 'Winter', 202223),
	(1423, 130, 'Spring', 202223),
	(1424, 130, 'Summer', 202223),
	(1425, 130, 'Fall', 202324),
	(1426, 130, 'Winter', 202324),
	(1427, 130, 'Spring', 202324),
	(1428, 130, 'Summer', 202324),
	(1429, 130, 'Fall', 202425),
	(1430, 130, 'Winter', 202425),
	(1431, 130, 'Spring', 202425),
	(1432, 130, 'Summer', 202425),
	(1433, 130, 'Completed', 202122),
	(1450, 131, 'Completed', 202122),
	(1451, 131, 'Fall', 202223),
	(1452, 131, 'Winter', 202223),
	(1453, 131, 'Spring', 202223),
	(1454, 131, 'Summer', 202223),
	(1455, 131, 'Completed', 202223),
	(1456, 131, 'Fall', 202324),
	(1457, 131, 'Winter', 202324),
	(1458, 131, 'Spring', 202324),
	(1459, 131, 'Summer', 202324),
	(1460, 131, 'Completed', 202324),
	(1461, 132, 'Fall', 202122),
	(1462, 132, 'Winter', 202122),
	(1463, 132, 'Spring', 202122),
	(1464, 132, 'Summer', 202122),
	(1465, 132, 'Fall', 202223),
	(1466, 132, 'Winter', 202223),
	(1467, 132, 'Spring', 202223),
	(1468, 132, 'Summer', 202223),
	(1469, 132, 'Fall', 202324),
	(1470, 132, 'Winter', 202324),
	(1471, 132, 'Spring', 202324),
	(1472, 132, 'Summer', 202324),
	(1473, 132, 'Fall', 202425),
	(1474, 132, 'Winter', 202425),
	(1475, 132, 'Spring', 202425),
	(1476, 132, 'Summer', 202425),
	(1477, 132, 'Completed', 202122),
	(1478, 133, 'Fall', 202122),
	(1479, 133, 'Winter', 202122),
	(1480, 133, 'Spring', 202122),
	(1481, 133, 'Summer', 202122),
	(1482, 133, 'Fall', 202223),
	(1483, 133, 'Winter', 202223),
	(1484, 133, 'Spring', 202223),
	(1485, 133, 'Summer', 202223),
	(1486, 133, 'Fall', 202324),
	(1487, 133, 'Winter', 202324),
	(1488, 133, 'Spring', 202324),
	(1489, 133, 'Summer', 202324),
	(1490, 133, 'Completed', 202122),
	(1491, 134, 'Fall', 202122),
	(1492, 134, 'Winter', 202122),
	(1493, 134, 'Spring', 202122),
	(1494, 134, 'Summer', 202122),
	(1495, 134, 'Fall', 202223),
	(1496, 134, 'Winter', 202223),
	(1497, 134, 'Spring', 202223),
	(1498, 134, 'Summer', 202223),
	(1499, 134, 'Fall', 202324),
	(1500, 134, 'Winter', 202324),
	(1501, 134, 'Spring', 202324),
	(1502, 134, 'Summer', 202324),
	(1503, 134, 'Fall', 202425),
	(1504, 134, 'Winter', 202425),
	(1505, 134, 'Spring', 202425),
	(1506, 134, 'Summer', 202425),
	(1507, 134, 'Completed', 202122),
	(1508, 135, 'Fall', 202122),
	(1509, 135, 'Winter', 202122),
	(1510, 135, 'Spring', 202122),
	(1511, 135, 'Summer', 202122),
	(1512, 135, 'Fall', 202223),
	(1513, 135, 'Winter', 202223),
	(1514, 135, 'Spring', 202223),
	(1515, 135, 'Summer', 202223),
	(1516, 135, 'Fall', 202324),
	(1517, 135, 'Winter', 202324),
	(1518, 135, 'Spring', 202324),
	(1519, 135, 'Summer', 202324),
	(1520, 135, 'Fall', 202425),
	(1521, 135, 'Winter', 202425),
	(1522, 135, 'Spring', 202425),
	(1523, 135, 'Summer', 202425),
	(1524, 135, 'Completed', 202122),
	(1525, 136, 'Fall', 202223),
	(1526, 136, 'Winter', 202223),
	(1527, 136, 'Spring', 202223),
	(1528, 136, 'Summer', 202223),
	(1529, 136, 'Fall', 202324),
	(1530, 136, 'Winter', 202324),
	(1531, 136, 'Spring', 202324),
	(1532, 136, 'Summer', 202324),
	(1533, 136, 'Fall', 202425),
	(1534, 136, 'Winter', 202425),
	(1535, 136, 'Spring', 202425),
	(1536, 136, 'Summer', 202425),
	(1537, 136, 'Fall', 202526),
	(1538, 136, 'Winter', 202526),
	(1539, 136, 'Spring', 202526),
	(1540, 136, 'Summer', 202526),
	(1541, 136, 'Completed', 202223),
	(1559, 138, 'Fall', 202223),
	(1560, 138, 'Winter', 202223),
	(1561, 138, 'Spring', 202223),
	(1562, 138, 'Summer', 202223),
	(1563, 138, 'Fall', 202324),
	(1564, 138, 'Winter', 202324),
	(1565, 138, 'Spring', 202324),
	(1566, 138, 'Summer', 202324),
	(1567, 138, 'Fall', 202425),
	(1568, 138, 'Winter', 202425),
	(1569, 138, 'Spring', 202425),
	(1570, 138, 'Summer', 202425),
	(1571, 138, 'Fall', 202526),
	(1572, 138, 'Winter', 202526),
	(1573, 138, 'Spring', 202526),
	(1574, 138, 'Summer', 202526),
	(1575, 138, 'Completed', 202223),
	(1576, 139, 'Fall', 201920),
	(1577, 139, 'Winter', 201920),
	(1578, 139, 'Spring', 201920),
	(1579, 139, 'Summer', 201920),
	(1580, 139, 'Fall', 202021),
	(1581, 139, 'Winter', 202021),
	(1582, 139, 'Spring', 202021),
	(1583, 139, 'Summer', 202021),
	(1584, 139, 'Fall', 202122),
	(1585, 139, 'Winter', 202122),
	(1586, 139, 'Spring', 202122),
	(1587, 139, 'Summer', 202122),
	(1596, 139, 'Completed', 201920),
	(1601, 139, 'Completed', 202425),
	(1619, 141, 'Fall', 202223),
	(1620, 141, 'Winter', 202223),
	(1621, 141, 'Spring', 202223),
	(1622, 141, 'Summer', 202223),
	(1623, 141, 'Fall', 202324),
	(1624, 141, 'Winter', 202324),
	(1625, 141, 'Spring', 202324),
	(1626, 141, 'Summer', 202324),
	(1627, 141, 'Fall', 202425),
	(1628, 141, 'Winter', 202425),
	(1629, 141, 'Spring', 202425),
	(1630, 141, 'Summer', 202425),
	(1631, 141, 'Fall', 202526),
	(1632, 141, 'Winter', 202526),
	(1633, 141, 'Spring', 202526),
	(1634, 141, 'Summer', 202526),
	(1635, 141, 'Completed', 202223),
	(1702, 145, 'Fall', 202021),
	(1703, 145, 'Winter', 202021),
	(1704, 145, 'Spring', 202021),
	(1705, 145, 'Summer', 202021),
	(1706, 145, 'Fall', 202122),
	(1707, 145, 'Winter', 202122),
	(1708, 145, 'Spring', 202122),
	(1709, 145, 'Summer', 202122),
	(1710, 145, 'Fall', 202223),
	(1711, 145, 'Winter', 202223),
	(1712, 145, 'Spring', 202223),
	(1713, 145, 'Summer', 202223),
	(1714, 145, 'Fall', 202324),
	(1715, 145, 'Winter', 202324),
	(1716, 145, 'Spring', 202324),
	(1717, 145, 'Summer', 202324),
	(1718, 145, 'Completed', 202021),
	(1744, 146, 'Fall', 202122),
	(1745, 146, 'Winter', 202122),
	(1746, 146, 'Spring', 202122),
	(1747, 146, 'Summer', 202122),
	(1748, 146, 'Fall', 202223),
	(1749, 146, 'Winter', 202223),
	(1750, 146, 'Spring', 202223),
	(1751, 146, 'Summer', 202223),
	(1752, 146, 'Fall', 202324),
	(1753, 146, 'Winter', 202324),
	(1754, 146, 'Spring', 202324),
	(1755, 146, 'Summer', 202324),
	(1756, 146, 'Fall', 202425),
	(1757, 146, 'Winter', 202425),
	(1758, 146, 'Spring', 202425),
	(1759, 146, 'Summer', 202425),
	(1760, 146, 'Completed', 202122),
	(1959, 156, 'Fall', 202223),
	(1960, 156, 'Winter', 202223),
	(1961, 156, 'Spring', 202223),
	(1962, 156, 'Summer', 202223),
	(1963, 156, 'Fall', 202324),
	(1964, 156, 'Winter', 202324),
	(1965, 156, 'Spring', 202324),
	(1966, 156, 'Summer', 202324),
	(1967, 156, 'Fall', 202425),
	(1968, 156, 'Winter', 202425),
	(1969, 156, 'Spring', 202425),
	(1970, 156, 'Summer', 202425),
	(1971, 156, 'Fall', 202526),
	(1972, 156, 'Winter', 202526),
	(1973, 156, 'Spring', 202526),
	(1974, 156, 'Summer', 202526),
	(1975, 156, 'Completed', 202223),
	(1976, 157, 'Fall', 202223),
	(1977, 157, 'Winter', 202223),
	(1978, 157, 'Spring', 202223),
	(1979, 157, 'Summer', 202223),
	(1980, 157, 'Fall', 202324),
	(1981, 157, 'Winter', 202324),
	(1982, 157, 'Spring', 202324),
	(1983, 157, 'Summer', 202324),
	(1992, 157, 'Completed', 202223),
	(2107, 164, 'Fall', 202526),
	(2108, 164, 'Winter', 202526),
	(2109, 164, 'Spring', 202526),
	(2110, 164, 'Summer', 202526),
	(2111, 164, 'Completed', 202223),
	(2167, 157, 'Completed', 202526),
	(2168, 157, 'Fall', 202627),
	(2169, 157, 'Winter', 202627),
	(2170, 157, 'Spring', 202627),
	(2171, 157, 'Summer', 202627),
	(2172, 157, 'Completed', 202627),
	(2177, 157, 'Completed', 202728),
	(2182, 157, 'Completed', 202829),
	(2187, 157, 'Completed', 202930),
	(2188, 157, 'Fall', 203031),
	(2189, 157, 'Winter', 203031),
	(2190, 157, 'Spring', 203031),
	(2191, 157, 'Summer', 203031),
	(2192, 157, 'Completed', 203031),
	(2210, 169, 'Fall', 202223),
	(2211, 169, 'Winter', 202223),
	(2212, 169, 'Spring', 202223),
	(2213, 169, 'Summer', 202223),
	(2214, 169, 'Fall', 202324),
	(2215, 169, 'Winter', 202324),
	(2216, 169, 'Spring', 202324),
	(2217, 169, 'Summer', 202324),
	(2218, 169, 'Fall', 202425),
	(2219, 169, 'Winter', 202425),
	(2220, 169, 'Spring', 202425),
	(2221, 169, 'Summer', 202425),
	(2226, 169, 'Completed', 202223),
	(2227, 169, 'Fall', 202627),
	(2228, 169, 'Winter', 202627),
	(2229, 169, 'Spring', 202627),
	(2230, 169, 'Summer', 202627),
	(2231, 169, 'Completed', 202627),
	(2266, 172, 'Fall', 202223),
	(2267, 172, 'Winter', 202223),
	(2268, 172, 'Spring', 202223),
	(2269, 172, 'Summer', 202223),
	(2270, 172, 'Fall', 202324),
	(2271, 172, 'Winter', 202324),
	(2272, 172, 'Spring', 202324),
	(2273, 172, 'Summer', 202324),
	(2274, 172, 'Fall', 202425),
	(2275, 172, 'Winter', 202425),
	(2276, 172, 'Spring', 202425),
	(2277, 172, 'Summer', 202425),
	(2278, 172, 'Fall', 202526),
	(2279, 172, 'Winter', 202526),
	(2280, 172, 'Spring', 202526),
	(2281, 172, 'Summer', 202526),
	(2282, 172, 'Completed', 202223);
/*!40000 ALTER TABLE `SemesterPlan` ENABLE KEYS */;

-- Dumping structure for table course_planner.StudentAdvisor
DROP TABLE IF EXISTS `StudentAdvisor`;
CREATE TABLE IF NOT EXISTS `StudentAdvisor` (
  `studentPersonID` int NOT NULL,
  `advisorPersonID` int NOT NULL,
  PRIMARY KEY (`studentPersonID`,`advisorPersonID`),
  KEY `fk_advisorPersonID` (`advisorPersonID`),
  CONSTRAINT `fk_advisorPersonID` FOREIGN KEY (`advisorPersonID`) REFERENCES `Person` (`personID`) ON DELETE CASCADE,
  CONSTRAINT `fk_studentPersonID` FOREIGN KEY (`studentPersonID`) REFERENCES `Person` (`personID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table course_planner.StudentAdvisor: ~0 rows (approximately)
/*!40000 ALTER TABLE `StudentAdvisor` DISABLE KEYS */;
/*!40000 ALTER TABLE `StudentAdvisor` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
