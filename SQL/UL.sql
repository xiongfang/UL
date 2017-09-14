-- MySQL dump 10.13  Distrib 5.7.9, for Win64 (x86_64)
--
-- Host: localhost    Database: ul
-- ------------------------------------------------------
-- Server version	5.7.10-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `member`
--

DROP TABLE IF EXISTS `member`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `member` (
  `declaring_type` varchar(100) NOT NULL,
  `name` varchar(256) NOT NULL,
  `is_static` bit(1) NOT NULL,
  `modifier` int(11) NOT NULL,
  `comments` varchar(4096) NOT NULL,
  `member_type` int(11) NOT NULL,
  `ext` varchar(4096) NOT NULL,
  `identifier` varchar(45) NOT NULL,
  `field_type_fullname` varchar(100) DEFAULT NULL,
  `method_args` varchar(4096) DEFAULT NULL,
  `method_ret_type` varchar(100) DEFAULT NULL,
  `method_body` varchar(4096) DEFAULT NULL,
  PRIMARY KEY (`identifier`,`declaring_type`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='类成员';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `member`
--

LOCK TABLES `member` WRITE;
/*!40000 ALTER TABLE `member` DISABLE KEYS */;
INSERT INTO `member` VALUES ('HelloWorld.Program','a','\0',0,'',4,'','a','System.Int32','null','','null'),('HelloWorld.Program','Main','\0',0,'',4,'','Main','','[{\"type_fullname\":\"System.String\",\"name\":\"arg\",\"is_ref\":false,\"is_out\":false,\"default_value\":\"\"}]','','null'),('HelloWorld.Program','Print','\0',0,'',4,'','Print','','[{\"type_fullname\":\"HelloWorld.TestInt\",\"name\":\"hello\",\"is_ref\":true,\"is_out\":false,\"default_value\":\"\"}]','','null');
/*!40000 ALTER TABLE `member` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `type`
--

DROP TABLE IF EXISTS `type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `type` (
  `full_name` varchar(100) NOT NULL,
  `comments` varchar(4096) NOT NULL,
  `modifier` int(11) NOT NULL,
  `is_abstract` bit(1) NOT NULL,
  `parent` varchar(256) NOT NULL,
  `is_interface` bit(1) NOT NULL,
  `imports` varchar(4096) NOT NULL,
  `ext` varchar(4096) NOT NULL,
  `is_value_type` bit(1) NOT NULL,
  PRIMARY KEY (`full_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='类型';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `type`
--

LOCK TABLES `type` WRITE;
/*!40000 ALTER TABLE `type` DISABLE KEYS */;
INSERT INTO `type` VALUES ('HelloWorld.Program','',0,'\0','','\0','','','\0'),('HelloWorld.TestInt','',0,'\0','','\0','','','\0'),('System.Int32','',0,'\0','','\0','','','\0'),('System.String','',0,'\0','','\0','','','\0');
/*!40000 ALTER TABLE `type` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2017-09-14 16:13:55
