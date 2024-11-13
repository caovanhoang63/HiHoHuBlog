CREATE DATABASE `blog`;
USE `blog`;

#   status:
#    -----------------------------------------------------------------
#   | code        |        meaning                                    |
#    -----------------------------------------------------------------
#   |   0         |        deleted                                    |
#   |   1         |        available                                  |
#   |   2         |        blocked                                    |
#   |   3         |                                                   |
#   |   4         |                                                   |
#   |   5         |                                                   |
#   |   6         |                                                   |
#   |   7         |                                                   |
#    -----------------------------------------------------------------
#

DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
                         `id` INT NOT NULL AUTO_INCREMENT,
                         `email` varchar(50) NOT NULL,
                         `user_name` varchar(50) NOT NULL,
                         `fb_id` varchar(50) DEFAULT NULL,
                         `gg_id` varchar(50) DEFAULT NULL,
                         `address` varchar(100) DEFAULT NULL,
                         `password` varchar(255) NOT NULL,
                         `salt` varchar(50) DEFAULT NULL,
                         `last_name` varchar(50) NOT NULL,
                         `first_name` varchar(50) NOT NULL,
                         `phone` varchar(20) DEFAULT NULL,
                         `role` enum('admin','mod','user') NOT NULL DEFAULT 'user',
                         `avatar` json DEFAULT NULL,
                         `status` INT NOT NULL DEFAULT '1',
                         `total_follower` INT NOT NULL DEFAULT 0,
                         `total_following` INT NOT NULL DEFAULT 0,
                         `total_mark` INT NOT NULL DEFAULT 0,
                         `total_comment` INT NOT NULL DEFAULT 0,
                         `total_blog` INT NOT NULL DEFAULT 0,
                         `total_blog_view` INT NOT NULL DEFAULT 0,
                         `created_at` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,
                         `updated_at` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
                         PRIMARY KEY (`id`),
                         KEY `status` (`status`) USING BTREE,
                         UNIQUE KEY `email` (`email`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8mb4;;

DROP TABLE IF EXISTS `user_follow`;
CREATE TABLE `user_follow` (
                               `user_id` INT,
                               `user_following` INT,
                               `created_at` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
                               PRIMARY KEY (`user_id`,`user_following`)
)ENGINE=InnoDB  DEFAULT CHARSET=utf8mb4;;

DROP TABLE IF EXISTS `user_details`;
CREATE TABLE `user_details` (
                                `id` INT,
                                `bio` varchar(255) DEFAULT NULL,
                                `status` INT NOT NULL DEFAULT '1',
                                `created_at` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,
                                `updated_at` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
                                PRIMARY KEY (`id`)
)ENGINE=InnoDB  DEFAULT CHARSET=utf8mb4;

SELECT * from blogs;

DROP TABLE IF EXISTS `blogs`;
CREATE TABLE `blogs` (
                         `id` INT NOT NULL AUTO_INCREMENT,
                         `user_id` INT NOT NULL,
                         `title` varchar(255) DEFAULT NULL,
                         `is_published` boolean,
                         `published_at` TIMESTAMP,
                         `min_to_read` INTEGER DEFAULT 0,  
                         `content` TEXT, 
                         `thumbnail` JSON DEFAULT NULL,
                         `total_view` INT NOT NULL DEFAULT 0,
                         `total_like` INT NOT NULL DEFAULT 0,
                         `total_mark` INT NOT NULL DEFAULT 0,
                         `status` INT NOT NULL DEFAULT '1',
                         `created_at` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,
                         `updated_at` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
                         PRIMARY KEY (`id`),
                         KEY `status` (`status`) USING BTREE
)ENGINE=InnoDB  DEFAULT CHARSET=utf8mb4;

DROP TABLE IF EXISTS `categories`;
CREATE TABLE  `categories` (
                               `id` INT NOT NULL AUTO_INCREMENT,
                               `status` INT NOT NULL DEFAULT '1',
                               `created_at` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,
                               `updated_at` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
                               PRIMARY KEY (`id`),
                               KEY `status` (`status`) USING BTREE
)ENGINE=InnoDB  DEFAULT CHARSET=utf8mb4;;

DROP TABLE IF EXISTS `blog_category`;
CREATE TABLE `blog_category` (
                                 `blog_id` INT ,
                                 `category_id` INT,
                                 `created_at` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,
                                 PRIMARY KEY (`blog_id`,`category_id`)
)ENGINE=InnoDB  DEFAULT CHARSET=utf8mb4;;

DROP TABLE IF EXISTS `user_like_blog`;
CREATE TABLE  `user_like_blog` (
                                   `user_id` INT,
                                   `blog_id` INT,
                                   `created_at` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
                                   PRIMARY KEY (`user_id`,`blog_id`)
)ENGINE=InnoDB  DEFAULT CHARSET=utf8mb4;;

DROP TABLE IF EXISTS `bookmarks`;
CREATE TABLE  `bookmarks` (
                              `user_id` INT,
                              `blog_id` INT,
                              `created_at` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
                              PRIMARY KEY (`user_id`,`blog_id`)
)ENGINE=InnoDB  DEFAULT CHARSET=utf8mb4;

DROP TABLE IF EXISTS `user_read_blogs`;
CREATE TABLE  `user_read_blogs` (
                                    `user_id` INT,
                                    `blog_id` INT,
                                    `created_at` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
                                    PRIMARY KEY (`user_id`,`blog_id`)
)ENGINE=InnoDB  DEFAULT CHARSET=utf8mb4;

DROP TABLE IF EXISTS `comments`;
CREATE TABLE `comments` (
                            `user_id` INT,
                            `blog_id` INT,
                            `content` text,
                            `created_at` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
                            PRIMARY KEY (`user_id`,`blog_id`)
)ENGINE=InnoDB  DEFAULT CHARSET=utf8mb4;



DROP TABLE IF EXISTS `blog_blocked`;
CREATE TABLE `blog_blocked` (
    `id` INT NOT NULL AUTO_INCREMENT,
    `blog_id` INT NOT NULL,
    `reason_blog_blocked_id` INT NOT NULL,
    `status` INT NOT NULL DEFAULT '1',
    `created_at` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    `updated_at` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`id`),
    KEY `status` (`status`) USING BTREE
)ENGINE=InnoDB  DEFAULT CHARSET=utf8mb4;
