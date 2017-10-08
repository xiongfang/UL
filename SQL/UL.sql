/*
SQLyog Ultimate v11.24 (64 bit)
MySQL - 5.5.25-MariaDB : Database - ul
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`ul` /*!40100 DEFAULT CHARACTER SET utf8 COLLATE utf8_bin */;

USE `ul`;

/*Table structure for table `member` */

DROP TABLE IF EXISTS `member`;

CREATE TABLE `member` (
  `declaring_type` varchar(1024) COLLATE utf8_bin NOT NULL,
  `name` varchar(256) COLLATE utf8_bin NOT NULL,
  `is_static` bit(1) NOT NULL,
  `modifier` int(11) NOT NULL,
  `comments` varchar(4096) COLLATE utf8_bin NOT NULL,
  `member_type` int(11) NOT NULL,
  `ext` varchar(4096) COLLATE utf8_bin NOT NULL,
  `identifier` varchar(1024) COLLATE utf8_bin NOT NULL,
  `field_type` varchar(1024) COLLATE utf8_bin NOT NULL,
  `method_args` varchar(4096) COLLATE utf8_bin NOT NULL,
  `method_ret_type` varchar(1024) COLLATE utf8_bin NOT NULL,
  `method_body` text CHARACTER SET utf8 NOT NULL,
  `order` int(11) NOT NULL,
  `field_initializer` text CHARACTER SET utf8 NOT NULL,
  `method_generic_parameter_definitions` text CHARACTER SET utf8 NOT NULL,
  `method_virtual` bit(1) NOT NULL,
  `method_override` bit(1) NOT NULL,
  `method_abstract` bit(1) NOT NULL,
  `attributes` varchar(2048) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin COMMENT='类成员';

/*Table structure for table `type` */

DROP TABLE IF EXISTS `type`;

CREATE TABLE `type` (
  `full_name` varchar(1024) COLLATE utf8mb4_bin NOT NULL,
  `comments` varchar(1024) COLLATE utf8mb4_bin NOT NULL,
  `modifier` int(11) NOT NULL,
  `is_abstract` bit(1) NOT NULL,
  `base_type` varchar(1024) COLLATE utf8mb4_bin NOT NULL,
  `is_interface` bit(1) NOT NULL,
  `ext` varchar(4096) COLLATE utf8mb4_bin NOT NULL,
  `is_value_type` bit(1) NOT NULL,
  `is_class` bit(1) NOT NULL,
  `interfaces` varchar(1024) COLLATE utf8mb4_bin NOT NULL,
  `is_generic_type_definition` bit(1) NOT NULL,
  `generic_parameter_definitions` varchar(1024) COLLATE utf8mb4_bin NOT NULL,
  `name` varchar(256) COLLATE utf8mb4_bin NOT NULL,
  `namespace` varchar(256) COLLATE utf8mb4_bin NOT NULL,
  `usingNamespace` varchar(1024) COLLATE utf8mb4_bin NOT NULL,
  `is_enum` bit(1) NOT NULL,
  `attributes` varchar(2048) COLLATE utf8mb4_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin COMMENT='类型';

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
