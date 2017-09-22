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
  `declaring_type` varchar(1024) NOT NULL,
  `name` varchar(256) NOT NULL,
  `is_static` bit(1) NOT NULL,
  `modifier` int(11) NOT NULL,
  `comments` varchar(4096) NOT NULL,
  `member_type` int(11) NOT NULL,
  `ext` varchar(4096) NOT NULL,
  `identifier` varchar(1024) NOT NULL,
  `field_type` varchar(1024) NOT NULL,
  `method_args` varchar(4096) NOT NULL,
  `method_ret_type` varchar(1024) NOT NULL,
  `method_body` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='类成员';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `member`
--

LOCK TABLES `member` WRITE;
/*!40000 ALTER TABLE `member` DISABLE KEYS */;
INSERT INTO `member` VALUES ('System.Int32','op_Equals','\0',0,'',8,'','op_Equals(System.Int32)','{\"identifer\":\"void\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":true}','[{\"type\":{\"identifer\":\"System.Int32\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false},\"name\":\"b\",\"is_ref\":false,\"is_out\":false,\"default_value\":\"\"}]','{\"identifer\":\"System.Int32\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false}','{\"List\":[{\"Expression\":{\"value\":\"1\",\"$Type\":\"Metadata.Expression.ConstExp\"},\"$Type\":\"Metadata.DB_ReturnStatementSyntax\"}],\"$Type\":\"Metadata.DB_BlockSyntax\"}'),('System.Int32','op_Small','\0',0,'',8,'','op_Small(System.Int32)','{\"identifer\":\"void\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":true}','[{\"type\":{\"identifer\":\"System.Int32\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false},\"name\":\"b\",\"is_ref\":false,\"is_out\":false,\"default_value\":\"\"}]','{\"identifer\":\"System.Int32\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false}','{\"List\":[{\"Expression\":{\"value\":\"1\",\"$Type\":\"Metadata.Expression.ConstExp\"},\"$Type\":\"Metadata.DB_ReturnStatementSyntax\"}],\"$Type\":\"Metadata.DB_BlockSyntax\"}'),('System.Int32','op_Assign','\0',0,'',8,'','op_Assign(System.Int32)','{\"identifer\":\"void\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":true}','[{\"type\":{\"identifer\":\"System.Int32\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false},\"name\":\"b\",\"is_ref\":false,\"is_out\":false,\"default_value\":\"\"}]','{\"identifer\":\"System.Int32\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false}','{\"List\":[{\"Expression\":{\"value\":\"1\",\"$Type\":\"Metadata.Expression.ConstExp\"},\"$Type\":\"Metadata.DB_ReturnStatementSyntax\"}],\"$Type\":\"Metadata.DB_BlockSyntax\"}'),('System.Console','WriteLine','',0,'',8,'','WriteLine(System.String)','{\"identifer\":\"void\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":true}','[{\"type\":{\"identifer\":\"System.String\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false},\"name\":\"v\",\"is_ref\":false,\"is_out\":false,\"default_value\":\"\"}]','{\"identifer\":\"void\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":true}','{\"List\":[],\"$Type\":\"Metadata.DB_BlockSyntax\"}'),('HelloWorld.TestGeneric[2]','a','\0',0,'',4,'','a','{\"identifer\":\"HelloWorld.TestGeneric[2]\",\"parameters\":[],\"template_parameter_name\":\"T1\",\"IsVoid\":false}','','{\"identifer\":\"void\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":true}',''),('HelloWorld.TestInt','a','\0',0,'',4,'','a','{\"identifer\":\"System.Int32\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false}','','{\"identifer\":\"void\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":true}',''),('HelloWorld.Program','a','',0,'',4,'','a','{\"identifer\":\"System.Int32\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false}','','{\"identifer\":\"void\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":true}',''),('HelloWorld.Program','v','\0',0,'',4,'','v','{\"identifer\":\"HelloWorld.TestGeneric[2]\",\"parameters\":[{\"identifer\":\"System.Int32\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false},{\"identifer\":\"System.String\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false}],\"template_parameter_name\":\"\",\"IsVoid\":false}','','{\"identifer\":\"void\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":true}',''),('HelloWorld.Program','s','\0',0,'',4,'','s','{\"identifer\":\"System.Array[1]\",\"parameters\":[{\"identifer\":\"System.Int32\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false}],\"template_parameter_name\":\"\",\"IsVoid\":false}','','{\"identifer\":\"void\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":true}',''),('HelloWorld.Program','Main','',1,'',8,'','Main(System.String,System.Int32)','{\"identifer\":\"void\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":true}','[{\"type\":{\"identifer\":\"System.String\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false},\"name\":\"arg\",\"is_ref\":false,\"is_out\":false,\"default_value\":\"\"},{\"type\":{\"identifer\":\"System.Int32\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false},\"name\":\"arg2\",\"is_ref\":false,\"is_out\":false,\"default_value\":\"\"}]','{\"identifer\":\"void\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":true}','{\"List\":[{\"Condition\":{\"value\":\"true\",\"$Type\":\"Metadata.Expression.ConstExp\"},\"Statement\":{\"List\":[{\"Exp\":{\"Caller\":{\"Caller\":{\"Name\":\"Console\",\"$Type\":\"Metadata.Expression.IndifierExp\"},\"Name\":\"WriteLine\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Args\":[{\"value\":\"\\\"Hello, World!\\\"\",\"$Type\":\"Metadata.Expression.ConstExp\"}],\"$Type\":\"Metadata.Expression.MethodExp\"},\"$Type\":\"Metadata.DB_ExpressionStatementSyntax\"}],\"$Type\":\"Metadata.DB_BlockSyntax\"},\"Else\":{\"List\":[{\"Exp\":{\"Caller\":{\"Caller\":{\"Name\":\"Console\",\"$Type\":\"Metadata.Expression.IndifierExp\"},\"Name\":\"WriteLine\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Args\":[{\"value\":\"\\\"Hello, World!1\\\"\",\"$Type\":\"Metadata.Expression.ConstExp\"}],\"$Type\":\"Metadata.Expression.MethodExp\"},\"$Type\":\"Metadata.DB_ExpressionStatementSyntax\"},{\"Exp\":{\"Caller\":{\"Caller\":{\"Name\":\"Console\",\"$Type\":\"Metadata.Expression.IndifierExp\"},\"Name\":\"WriteLine\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Args\":[{\"value\":\"\\\"Hello, World!2\\\"\",\"$Type\":\"Metadata.Expression.ConstExp\"}],\"$Type\":\"Metadata.Expression.MethodExp\"},\"$Type\":\"Metadata.DB_ExpressionStatementSyntax\"}],\"$Type\":\"Metadata.DB_BlockSyntax\"},\"$Type\":\"Metadata.DB_IfStatementSyntax\"}],\"$Type\":\"Metadata.DB_BlockSyntax\"}'),('HelloWorld.Program','Print','\0',1,'',8,'','Print(HelloWorld.TestInt)','{\"identifer\":\"void\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":true}','[{\"type\":{\"identifer\":\"HelloWorld.TestInt\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false},\"name\":\"hello\",\"is_ref\":true,\"is_out\":false,\"default_value\":\"\"}]','{\"identifer\":\"void\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":true}','{\"List\":[{\"Type\":{\"identifer\":\"HelloWorld.TestInt\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false},\"Variables\":[{\"Identifier\":\"c\",\"Initializer\":{\"Type\":{\"identifer\":\"HelloWorld.TestInt\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false},\"Args\":[],\"$Type\":\"Metadata.Expression.ObjectCreateExp\"},\"$Type\":\"Metadata.VariableDeclaratorSyntax\"}],\"$Type\":\"Metadata.DB_LocalDeclarationStatementSyntax\"},{\"Exp\":{\"Caller\":{\"Caller\":{\"Caller\":{\"Name\":\"c\",\"$Type\":\"Metadata.Expression.IndifierExp\"},\"Name\":\"a\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Name\":\"op_Equals\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Args\":[{\"value\":\"7\",\"$Type\":\"Metadata.Expression.ConstExp\"}],\"$Type\":\"Metadata.Expression.MethodExp\"},\"$Type\":\"Metadata.DB_ExpressionStatementSyntax\"},{\"Exp\":{\"Caller\":{\"Caller\":{\"Name\":\"Console\",\"$Type\":\"Metadata.Expression.IndifierExp\"},\"Name\":\"WriteLine\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Args\":[{\"value\":\"\\\"Print\\\"\",\"$Type\":\"Metadata.Expression.ConstExp\"}],\"$Type\":\"Metadata.Expression.MethodExp\"},\"$Type\":\"Metadata.DB_ExpressionStatementSyntax\"},{\"Type\":{\"identifer\":\"System.Array[1]\",\"parameters\":[{\"identifer\":\"System.Int32\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false}],\"template_parameter_name\":\"\",\"IsVoid\":false},\"Variables\":[{\"Identifier\":\"v\",\"Initializer\":{\"Type\":{\"identifer\":\"System.Array[1]\",\"parameters\":[{\"identifer\":\"System.Int32\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false}],\"template_parameter_name\":\"\",\"IsVoid\":false},\"Args\":[{\"value\":\"6\",\"$Type\":\"Metadata.Expression.ConstExp\"}],\"$Type\":\"Metadata.Expression.ObjectCreateExp\"},\"$Type\":\"Metadata.VariableDeclaratorSyntax\"}],\"$Type\":\"Metadata.DB_LocalDeclarationStatementSyntax\"},{\"Declaration\":{\"Type\":{\"identifer\":\"System.Int32\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false},\"Variables\":[{\"Identifier\":\"i\",\"Initializer\":{\"value\":\"0\",\"$Type\":\"Metadata.Expression.ConstExp\"},\"$Type\":\"Metadata.VariableDeclaratorSyntax\"},{\"Identifier\":\"c\",\"Initializer\":{\"value\":\"6\",\"$Type\":\"Metadata.Expression.ConstExp\"},\"$Type\":\"Metadata.VariableDeclaratorSyntax\"}],\"$Type\":\"Metadata.VariableDeclarationSyntax\"},\"Condition\":{\"Caller\":{\"Caller\":{\"Name\":\"i\",\"$Type\":\"Metadata.Expression.IndifierExp\"},\"Name\":\"op_Small\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Args\":[{\"value\":\"3\",\"$Type\":\"Metadata.Expression.ConstExp\"}],\"$Type\":\"Metadata.Expression.MethodExp\"},\"Incrementors\":[{\"Caller\":{\"Caller\":{\"Name\":\"i\",\"$Type\":\"Metadata.Expression.IndifierExp\"},\"Name\":\"op_Assign\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Args\":[{\"Caller\":{\"Caller\":{\"Name\":\"i\",\"$Type\":\"Metadata.Expression.IndifierExp\"},\"Name\":\"op_PlusPlus\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Args\":[{\"value\":\"1\",\"$Type\":\"Metadata.Expression.ConstExp\"}],\"$Type\":\"Metadata.Expression.MethodExp\"}],\"$Type\":\"Metadata.Expression.MethodExp\"}],\"Statement\":{\"List\":[],\"$Type\":\"Metadata.DB_BlockSyntax\"},\"$Type\":\"Metadata.DB_ForStatementSyntax\"},{\"Condition\":{\"value\":\"true\",\"$Type\":\"Metadata.Expression.ConstExp\"},\"Statement\":{\"List\":[{\"Exp\":{\"Caller\":{\"Caller\":{\"Name\":\"Console\",\"$Type\":\"Metadata.Expression.IndifierExp\"},\"Name\":\"WriteLine\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Args\":[{\"value\":\"\\\"DO\\\"\",\"$Type\":\"Metadata.Expression.ConstExp\"}],\"$Type\":\"Metadata.Expression.MethodExp\"},\"$Type\":\"Metadata.DB_ExpressionStatementSyntax\"}],\"$Type\":\"Metadata.DB_BlockSyntax\"},\"$Type\":\"Metadata.DB_DoStatementSyntax\"},{\"Condition\":{\"value\":\"true\",\"$Type\":\"Metadata.Expression.ConstExp\"},\"Statement\":{\"List\":[{\"Exp\":{\"Caller\":{\"Caller\":{\"Name\":\"Console\",\"$Type\":\"Metadata.Expression.IndifierExp\"},\"Name\":\"WriteLine\",\"$Type\":\"Metadata.Expression.FieldExp\"},\"Args\":[{\"value\":\"\\\"while\\\"\",\"$Type\":\"Metadata.Expression.ConstExp\"}],\"$Type\":\"Metadata.Expression.MethodExp\"},\"$Type\":\"Metadata.DB_ExpressionStatementSyntax\"}],\"$Type\":\"Metadata.DB_BlockSyntax\"},\"$Type\":\"Metadata.DB_WhileStatementSyntax\"},{\"Expression\":{\"value\":\"5\",\"$Type\":\"Metadata.Expression.ConstExp\"},\"Sections\":[{\"Labels\":[{\"value\":\"1\",\"$Type\":\"Metadata.Expression.ConstExp\"}],\"Statements\":[{\"$Type\":\"Metadata.DB_BreakStatementSyntax\"}]},{\"Labels\":[{\"value\":\"2\",\"$Type\":\"Metadata.Expression.ConstExp\"},{\"value\":\"3\",\"$Type\":\"Metadata.Expression.ConstExp\"}],\"Statements\":[{\"$Type\":\"Metadata.DB_BreakStatementSyntax\"}]}],\"$Type\":\"Metadata.DB_SwitchStatementSyntax\"},{\"Type\":{\"identifer\":\"HelloWorld.TestGeneric[2]\",\"parameters\":[{\"identifer\":\"System.Int32\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false},{\"identifer\":\"System.String\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false}],\"template_parameter_name\":\"\",\"IsVoid\":false},\"Variables\":[{\"Identifier\":\"temp\",\"Initializer\":{\"Type\":{\"identifer\":\"HelloWorld.TestGeneric[2]\",\"parameters\":[{\"identifer\":\"System.Int32\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false},{\"identifer\":\"System.String\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false}],\"template_parameter_name\":\"\",\"IsVoid\":false},\"Args\":[],\"$Type\":\"Metadata.Expression.ObjectCreateExp\"},\"$Type\":\"Metadata.VariableDeclaratorSyntax\"}],\"$Type\":\"Metadata.DB_LocalDeclarationStatementSyntax\"}],\"$Type\":\"Metadata.DB_BlockSyntax\"}');
/*!40000 ALTER TABLE `member` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `type`
--

DROP TABLE IF EXISTS `type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `type` (
  `full_name` varchar(1024) NOT NULL,
  `comments` varchar(1024) NOT NULL,
  `modifier` int(11) NOT NULL,
  `is_abstract` bit(1) NOT NULL,
  `base_type` varchar(1024) NOT NULL,
  `is_interface` bit(1) NOT NULL,
  `ext` varchar(4096) NOT NULL,
  `is_value_type` bit(1) NOT NULL,
  `is_class` bit(1) NOT NULL,
  `interfaces` varchar(1024) NOT NULL,
  `is_generic_type_definition` bit(1) NOT NULL,
  `generic_parameter_definitions` varchar(1024) NOT NULL,
  `name` varchar(256) NOT NULL,
  `namespace` varchar(256) NOT NULL,
  `usingNamespace` varchar(1024) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='类型';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `type`
--

LOCK TABLES `type` WRITE;
/*!40000 ALTER TABLE `type` DISABLE KEYS */;
INSERT INTO `type` VALUES ('System.Int32','',0,'\0','{\"identifer\":\"System.Object\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false}','\0','','\0','','[]','\0','[]','Int32','System','[]'),('System.String','',0,'\0','{\"identifer\":\"System.Object\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false}','\0','','\0','','[]','\0','[]','String','System','[]'),('System.Object','',0,'\0','{\"identifer\":\"void\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":true}','\0','','\0','','[]','\0','[]','Object','System','[]'),('System.Array[1]','',0,'\0','{\"identifer\":\"System.Object\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false}','\0','','\0','\0','[]','','[{\"type_name\":\"T\",\"constraint\":[]}]','Array','System','[]'),('System.Console','',0,'\0','{\"identifer\":\"System.Object\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false}','\0','','\0','','[]','\0','[]','Console','System','[]'),('HelloWorld.TestGeneric[2]','',0,'\0','{\"identifer\":\"System.Object\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false}','\0','','\0','\0','[]','','[{\"type_name\":\"T1\",\"constraint\":[\"HelloWorld.TestInt\"]},{\"type_name\":\"T2\",\"constraint\":[]}]','TestGeneric','HelloWorld','[\"System\"]'),('HelloWorld.TestInt','',0,'\0','{\"identifer\":\"System.Object\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false}','\0','','\0','','[]','\0','[]','TestInt','HelloWorld','[\"System\"]'),('HelloWorld.Program','',0,'\0','{\"identifer\":\"System.Object\",\"parameters\":[],\"template_parameter_name\":\"\",\"IsVoid\":false}','\0','','\0','','[]','\0','[]','Program','HelloWorld','[\"System\"]');
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

-- Dump completed on 2017-09-22 16:54:37
