/*
Navicat MySQL Data Transfer

Source Server         : root
Source Server Version : 50719
Source Host           : localhost:3306
Source Database       : test

Target Server Type    : MYSQL
Target Server Version : 50719
File Encoding         : 65001

Date: 2018-05-08 15:46:46
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for cnblognews
-- ----------------------------
DROP TABLE IF EXISTS `cnblognews`;
CREATE TABLE `cnblognews` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `author` varchar(255) DEFAULT NULL,
  `watch` varchar(255) DEFAULT NULL,
  `comment` varchar(255) DEFAULT NULL,
  `favorite` varchar(255) DEFAULT NULL,
  `content` text,
  `url` varchar(255) DEFAULT NULL,
  `title` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for showstart
-- ----------------------------
DROP TABLE IF EXISTS `showstart`;
CREATE TABLE `showstart` (
  `id` int(11) unsigned zerofill NOT NULL AUTO_INCREMENT,
  `showname` varchar(255) DEFAULT '',
  `actor` varchar(255) DEFAULT '',
  `price` varchar(255) DEFAULT '',
  `time` varchar(255) DEFAULT NULL,
  `place` varchar(255) DEFAULT '',
  `url` varchar(255) DEFAULT '',
  `type` varchar(255) DEFAULT '',
  `StartOrEnd` tinyint(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3520 DEFAULT CHARSET=utf8;
