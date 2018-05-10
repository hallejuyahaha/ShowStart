/*
Navicat MySQL Data Transfer

Source Server         : root
Source Server Version : 50719
Source Host           : localhost:3306
Source Database       : test

Target Server Type    : MYSQL
Target Server Version : 50719
File Encoding         : 65001

Date: 2018-05-10 09:21:22
*/

SET FOREIGN_KEY_CHECKS=0;

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
  `ReadSessionTime` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6234 DEFAULT CHARSET=utf8;
