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
INSERT INTO `member` VALUES ('HelloWorld.Program','a','',0,'',4,'','a','System.Int32','','',''),('HelloWorld.TestInt','a','\0',0,'',4,'','a','System.Int32','','',''),('HelloWorld.Program','Main','',1,'',8,'','Main(System.String,System.Int32)','','[{\"type_fullname\":\"System.String\",\"name\":\"arg\",\"is_ref\":false,\"is_out\":false,\"default_value\":\"\"},{\"type_fullname\":\"System.Int32\",\"name\":\"arg2\",\"is_ref\":false,\"is_out\":false,\"default_value\":\"\"}]','','{\"List\":[{\"Condition\":{\"value\":\"true\",\"$Type\":\"Metadata.Expression.ConstExp\"},\"Statement\":{\"List\":[{\"Exp\":{\"Caller\":{\"Caller\":{\"Name\":\"Console\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Name\":\"WriteLine\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Args\":[{\"value\":\"\\\"Hello, World!\\\"\",\"$Type\":\"Metadata.Expression.ConstExp\"}],\"$Type\":\"Metadata.Expression.MethodExp\"},\"$Type\":\"Metadata.DB_ExpressionStatementSyntax\"}],\"$Type\":\"Metadata.DB_BlockSyntax\"},\"Else\":{\"List\":[{\"Exp\":{\"Caller\":{\"Caller\":{\"Name\":\"Console\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Name\":\"WriteLine\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Args\":[{\"value\":\"\\\"Hello, World!1\\\"\",\"$Type\":\"Metadata.Expression.ConstExp\"}],\"$Type\":\"Metadata.Expression.MethodExp\"},\"$Type\":\"Metadata.DB_ExpressionStatementSyntax\"},{\"Exp\":{\"Caller\":{\"Caller\":{\"Name\":\"Console\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Name\":\"WriteLine\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Args\":[{\"value\":\"\\\"Hello, World!2\\\"\",\"$Type\":\"Metadata.Expression.ConstExp\"}],\"$Type\":\"Metadata.Expression.MethodExp\"},\"$Type\":\"Metadata.DB_ExpressionStatementSyntax\"}],\"$Type\":\"Metadata.DB_BlockSyntax\"},\"$Type\":\"Metadata.DB_IfStatementSyntax\"}],\"$Type\":\"Metadata.DB_BlockSyntax\"}'),('HelloWorld.Program','Print','\0',1,'',8,'','Print(HelloWorld.TestInt)','','[{\"type_fullname\":\"HelloWorld.TestInt\",\"name\":\"hello\",\"is_ref\":true,\"is_out\":false,\"default_value\":\"\"}]','','{\"List\":[{\"Type\":\"HelloWorld.TestInt\",\"Variables\":[{\"Identifier\":\"c\",\"Initializer\":{\"Type\":\"HelloWorld.TestInt\",\"Args\":[],\"$Type\":\"Metadata.Expression.ObjectCreateExp\"},\"$Type\":\"Metadata.VariableDeclaratorSyntax\"}],\"$Type\":\"Metadata.DB_LocalDeclarationStatementSyntax\"},{\"Exp\":{\"Caller\":{\"Caller\":{\"Caller\":{\"Name\":\"c\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Name\":\"a\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Name\":\"op_Equals\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Args\":[{\"value\":\"7\",\"$Type\":\"Metadata.Expression.ConstExp\"}],\"$Type\":\"Metadata.Expression.MethodExp\"},\"$Type\":\"Metadata.DB_ExpressionStatementSyntax\"},{\"Exp\":{\"Caller\":{\"Caller\":{\"Name\":\"Console\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Name\":\"WriteLine\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Args\":[{\"value\":\"\\\"Print\\\"\",\"$Type\":\"Metadata.Expression.ConstExp\"}],\"$Type\":\"Metadata.Expression.MethodExp\"},\"$Type\":\"Metadata.DB_ExpressionStatementSyntax\"},{\"Declaration\":{\"Type\":\"System.Int32\",\"Variables\":[{\"Identifier\":\"i\",\"Initializer\":{\"value\":\"0\",\"$Type\":\"Metadata.Expression.ConstExp\"},\"$Type\":\"Metadata.VariableDeclaratorSyntax\"},{\"Identifier\":\"c\",\"Initializer\":{\"value\":\"6\",\"$Type\":\"Metadata.Expression.ConstExp\"},\"$Type\":\"Metadata.VariableDeclaratorSyntax\"}],\"$Type\":\"Metadata.VariableDeclarationSyntax\"},\"Condition\":{\"Caller\":{\"Caller\":{\"Name\":\"i\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Name\":\"op_Small\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Args\":[{\"value\":\"3\",\"$Type\":\"Metadata.Expression.ConstExp\"}],\"$Type\":\"Metadata.Expression.MethodExp\"},\"Incrementors\":[{\"Caller\":{\"Caller\":{\"Name\":\"i\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Name\":\"op_Assign\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Args\":[{\"Caller\":{\"Caller\":{\"Name\":\"i\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Name\":\"op_PlusPlus\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Args\":[{\"value\":\"1\",\"$Type\":\"Metadata.Expression.ConstExp\"}],\"$Type\":\"Metadata.Expression.MethodExp\"}],\"$Type\":\"Metadata.Expression.MethodExp\"}],\"Statement\":{\"List\":[],\"$Type\":\"Metadata.DB_BlockSyntax\"},\"$Type\":\"Metadata.DB_ForStatementSyntax\"},{\"Condition\":{\"value\":\"true\",\"$Type\":\"Metadata.Expression.ConstExp\"},\"Statement\":{\"List\":[{\"Exp\":{\"Caller\":{\"Caller\":{\"Name\":\"Console\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Name\":\"WriteLine\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Args\":[{\"value\":\"\\\"DO\\\"\",\"$Type\":\"Metadata.Expression.ConstExp\"}],\"$Type\":\"Metadata.Expression.MethodExp\"},\"$Type\":\"Metadata.DB_ExpressionStatementSyntax\"}],\"$Type\":\"Metadata.DB_BlockSyntax\"},\"$Type\":\"Metadata.DB_DoStatementSyntax\"},{\"Condition\":{\"value\":\"true\",\"$Type\":\"Metadata.Expression.ConstExp\"},\"Statement\":{\"List\":[{\"Exp\":{\"Caller\":{\"Caller\":{\"Name\":\"Console\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Name\":\"WriteLine\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Args\":[{\"value\":\"\\\"while\\\"\",\"$Type\":\"Metadata.Expression.ConstExp\"}],\"$Type\":\"Metadata.Expression.MethodExp\"},\"$Type\":\"Metadata.DB_ExpressionStatementSyntax\"}],\"$Type\":\"Metadata.DB_BlockSyntax\"},\"$Type\":\"Metadata.DB_WhileStatementSyntax\"},{\"Expression\":{\"value\":\"5\",\"$Type\":\"Metadata.Expression.ConstExp\"},\"Sections\":[{\"Labels\":[{\"value\":\"1\",\"$Type\":\"Metadata.Expression.ConstExp\"}],\"Statements\":[{\"$Type\":\"Metadata.DB_BreakStatementSyntax\"}]},{\"Labels\":[{\"value\":\"2\",\"$Type\":\"Metadata.Expression.ConstExp\"},{\"value\":\"3\",\"$Type\":\"Metadata.Expression.ConstExp\"}],\"Statements\":[{\"$Type\":\"Metadata.DB_BreakStatementSyntax\"}]}],\"$Type\":\"Metadata.DB_SwitchStatementSyntax\"},{\"Expression\":{\"value\":\"\\\"5\\\"\",\"$Type\":\"Metadata.Expression.ConstExp\"},\"Sections\":[{\"Labels\":[{\"value\":\"\\\"haha\\\"\",\"$Type\":\"Metadata.Expression.ConstExp\"}],\"Statements\":[{\"$Type\":\"Metadata.DB_BreakStatementSyntax\"}]},{\"Labels\":[{\"Caller\":{\"Name\":\"TestE\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Name\":\"B\",\"$Type\":\"Metadata.Expression.FieldExp\"},{\"Caller\":{\"Name\":\"TestE\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Name\":\"C\",\"$Type\":\"Metadata.Expression.FieldExp\"}],\"Statements\":[{\"$Type\":\"Metadata.DB_BreakStatementSyntax\"}]}],\"$Type\":\"Metadata.DB_SwitchStatementSyntax\"}],\"$Type\":\"Metadata.DB_BlockSyntax\"}');
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
  `comments` varchar(1024) NOT NULL,
  `modifier` int(11) NOT NULL,
  `is_abstract` bit(1) NOT NULL,
  `base_type` varchar(256) NOT NULL,
  `is_interface` bit(1) NOT NULL,
  `ext` varchar(4096) NOT NULL,
  `is_value_type` bit(1) NOT NULL,
  `is_class` bit(1) NOT NULL,
  `interfaces` varchar(1024) NOT NULL,
  `is_generic_type_definition` bit(1) NOT NULL,
  `generic_parameter_definitions` varchar(1024) NOT NULL,
  PRIMARY KEY (`full_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='类型';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `type`
--

LOCK TABLES `type` WRITE;
/*!40000 ALTER TABLE `type` DISABLE KEYS */;
INSERT INTO `type` VALUES ('HelloWorld.Program','',0,'\0','','\0','','\0','','[]','\0','[]'),('HelloWorld.TestGeneric<T1,T2>','',0,'\0','','\0','','\0','\0','[]','','[{\"type_name\":\"T1\",\"constraint\":[]},{\"type_name\":\"T2\",\"constraint\":[]}]'),('HelloWorld.TestInt','',0,'\0','','\0','','\0','','[]','\0','[]'),('System.Int32','',0,'\0','','\0','','\0','','[]','\0','[]'),('System.String','',0,'\0','','\0','','\0','','[]','\0','[]');
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

-- Dump completed on 2017-09-18 12:54:28
