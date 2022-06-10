-- --------------------------------------------------------
-- 호스트:                          127.0.0.1
-- 서버 버전:                        10.7.3-MariaDB - mariadb.org binary distribution
-- 서버 OS:                        Win64
-- HeidiSQL 버전:                  11.3.0.6295
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- kyowontoy 데이터베이스 구조 내보내기
CREATE DATABASE IF NOT EXISTS `kyowontoy` /*!40100 DEFAULT CHARACTER SET utf8mb3 */;
USE `kyowontoy`;

-- 테이블 kyowontoy.board 구조 내보내기
CREATE TABLE IF NOT EXISTS `board` (
  `idx` int(11) NOT NULL AUTO_INCREMENT,
  `title` varchar(100) NOT NULL,
  `contents` longtext NOT NULL,
  `user` int(11) NOT NULL,
  `username` varchar(50) NOT NULL,
  `registeredDate` datetime NOT NULL,
  `view_cnt` int(10) unsigned NOT NULL DEFAULT 0,
  `active` smallint(5) NOT NULL,
  `type` smallint(6) NOT NULL,
  `member_seq` int(11) DEFAULT NULL,
  `like` int(11) DEFAULT NULL,
  PRIMARY KEY (`idx`) USING BTREE,
  KEY `FK_board_member` (`member_seq`),
  CONSTRAINT `FK_board_member` FOREIGN KEY (`member_seq`) REFERENCES `member` (`member_seq`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=319 DEFAULT CHARSET=utf8mb3 ROW_FORMAT=DYNAMIC COMMENT='table';

-- 내보낼 데이터가 선택되어 있지 않습니다.

-- 테이블 kyowontoy.boardcheck 구조 내보내기
CREATE TABLE IF NOT EXISTS `boardcheck` (
  `idx` int(11) NOT NULL AUTO_INCREMENT,
  `registeredDate` datetime NOT NULL,
  `member_seq` int(11) DEFAULT NULL,
  `board_idx` int(11) DEFAULT NULL,
  PRIMARY KEY (`idx`),
  KEY `FK_boardcheck_board` (`board_idx`),
  KEY `FK_boardcheck_member` (`member_seq`),
  CONSTRAINT `FK_boardcheck_board` FOREIGN KEY (`board_idx`) REFERENCES `board` (`idx`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_boardcheck_member` FOREIGN KEY (`member_seq`) REFERENCES `member` (`member_seq`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=74 DEFAULT CHARSET=utf8mb3 COMMENT='조회자 정보 테이블';

-- 내보낼 데이터가 선택되어 있지 않습니다.

-- 테이블 kyowontoy.boardlike 구조 내보내기
CREATE TABLE IF NOT EXISTS `boardlike` (
  `idx` int(11) NOT NULL AUTO_INCREMENT,
  `member_seq` int(11) DEFAULT NULL,
  `board_idx` int(11) DEFAULT NULL,
  `likeOrNot` int(11) DEFAULT NULL,
  PRIMARY KEY (`idx`),
  KEY `FK_boardlike_board` (`board_idx`),
  KEY `FK_boardlike_member` (`member_seq`),
  CONSTRAINT `FK_boardlike_board` FOREIGN KEY (`board_idx`) REFERENCES `board` (`idx`),
  CONSTRAINT `FK_boardlike_member` FOREIGN KEY (`member_seq`) REFERENCES `member` (`member_seq`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb3;

-- 내보낼 데이터가 선택되어 있지 않습니다.

-- 테이블 kyowontoy.comment 구조 내보내기
CREATE TABLE IF NOT EXISTS `comment` (
  `idx` int(11) NOT NULL AUTO_INCREMENT,
  `board_idx` int(11) DEFAULT 0,
  `member_seq` int(11) DEFAULT 0,
  `content` varchar(100) DEFAULT NULL,
  `registeredDate` datetime DEFAULT NULL,
  `active` int(11) DEFAULT 0,
  PRIMARY KEY (`idx`),
  KEY `FK_board` (`board_idx`),
  KEY `FK_comment_member` (`member_seq`),
  CONSTRAINT `FK_board` FOREIGN KEY (`board_idx`) REFERENCES `board` (`idx`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_comment_member` FOREIGN KEY (`member_seq`) REFERENCES `member` (`member_seq`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=79 DEFAULT CHARSET=utf8mb3;

-- 내보낼 데이터가 선택되어 있지 않습니다.

-- 테이블 kyowontoy.file 구조 내보내기
CREATE TABLE IF NOT EXISTS `file` (
  `idx` int(11) NOT NULL AUTO_INCREMENT,
  `fileName` varchar(255) DEFAULT NULL,
  `fileUrl` varchar(255) DEFAULT NULL,
  `board_idx` int(11) DEFAULT NULL,
  PRIMARY KEY (`idx`),
  KEY `FK_file_board` (`board_idx`),
  CONSTRAINT `FK_file_board` FOREIGN KEY (`board_idx`) REFERENCES `board` (`idx`)
) ENGINE=InnoDB AUTO_INCREMENT=39 DEFAULT CHARSET=utf8mb3;

-- 내보낼 데이터가 선택되어 있지 않습니다.

-- 테이블 kyowontoy.mailbox 구조 내보내기
CREATE TABLE IF NOT EXISTS `mailbox` (
  `idx` int(11) NOT NULL AUTO_INCREMENT,
  `sender_idx` int(11) NOT NULL,
  `receiver_idx` int(11) NOT NULL,
  `title` varchar(50) NOT NULL,
  `content` text NOT NULL,
  `sent_time` datetime NOT NULL DEFAULT current_timestamp(),
  `checked_time` datetime DEFAULT NULL,
  `active` int(11) DEFAULT NULL,
  PRIMARY KEY (`idx`),
  KEY `FK_mailbox_member` (`sender_idx`),
  KEY `FK_mailbox_member_2` (`receiver_idx`),
  CONSTRAINT `FK_mailbox_member` FOREIGN KEY (`sender_idx`) REFERENCES `member` (`member_seq`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_mailbox_member_2` FOREIGN KEY (`receiver_idx`) REFERENCES `member` (`member_seq`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=71 DEFAULT CHARSET=utf8mb3;

-- 내보낼 데이터가 선택되어 있지 않습니다.

-- 테이블 kyowontoy.mailboxfile 구조 내보내기
CREATE TABLE IF NOT EXISTS `mailboxfile` (
  `idx` int(11) NOT NULL AUTO_INCREMENT,
  `fileName` varchar(255) DEFAULT NULL,
  `fileUrl` varchar(255) DEFAULT NULL,
  `member_seq` int(11) DEFAULT NULL,
  `mailbox_idx` int(11) DEFAULT NULL,
  PRIMARY KEY (`idx`),
  KEY `FK_mailboxfile_mailbox` (`mailbox_idx`),
  KEY `FK_mailboxfile_member` (`member_seq`),
  CONSTRAINT `FK_mailboxfile_mailbox` FOREIGN KEY (`mailbox_idx`) REFERENCES `mailbox` (`idx`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_mailboxfile_member` FOREIGN KEY (`member_seq`) REFERENCES `member` (`member_seq`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb3;

-- 내보낼 데이터가 선택되어 있지 않습니다.

-- 테이블 kyowontoy.member 구조 내보내기
CREATE TABLE IF NOT EXISTS `member` (
  `member_seq` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  `password` varchar(200) NOT NULL,
  `department` varchar(20) DEFAULT NULL,
  `position` varchar(50) DEFAULT NULL,
  `photo` varchar(255) DEFAULT NULL,
  `registeredDate` date DEFAULT NULL,
  `withdrawalDate` date DEFAULT NULL,
  `email` varchar(50) DEFAULT NULL,
  `office_tel` varchar(50) DEFAULT NULL,
  `mobile_tel` varchar(50) DEFAULT NULL,
  `address` longtext DEFAULT NULL,
  `active` int(11) DEFAULT NULL,
  `birthDay` date DEFAULT NULL,
  `grade` int(11) DEFAULT NULL,
  `mainwork` text DEFAULT NULL,
  PRIMARY KEY (`member_seq`)
) ENGINE=InnoDB AUTO_INCREMENT=10100 DEFAULT CHARSET=utf8mb3;

-- 내보낼 데이터가 선택되어 있지 않습니다.

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
