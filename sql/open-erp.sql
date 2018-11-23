CREATE SCHEMA `organization` DEFAULT CHARACTER SET utf8 ;

CREATE TABLE `organization`.`organization` (
  `idorganization` INT NOT NULL AUTO_INCREMENT,
  `uuid` VARCHAR(45) NOT NULL,
  `node` VARCHAR(45) NULL COMMENT 'node name or sub-organization name',
  `type` VARCHAR(45) NULL COMMENT 'node type :root, leaf',
  `parents` VARCHAR(45) NULL COMMENT 'node parents uuid for organization tree ',
  `firstchild` VARCHAR(45) NULL COMMENT 'firstchild uuid for organization tree ',
  `nextsibling` VARCHAR(45) NULL COMMENT 'nextsibling uuid for organization tree ',
  `name` VARCHAR(45) NULL,
  `value_type` VARCHAR(45) NULL COMMENT 'node value ',
  `value_uuid` VARCHAR(45) NULL COMMENT 'uuid_link  link to other database or table ',
  `value_url` VARCHAR(200) NULL COMMENT 'value point one url ',
  `value` VARCHAR(200) NULL COMMENT 'value text ',
  `time` DATETIME NOT NULL DEFAULT NOW(),
  `status` VARCHAR(45) NOT NULL DEFAULT 'normal',
  `priority` INT NULL,
  `author` VARCHAR(45) NULL COMMENT 'This node record creator,can name,can app ,',
  `location` VARCHAR(45) NULL COMMENT 'The location where this record was created ,Latitude, latitude, city, address and so on ...',
  `remark` TEXT NULL,
  PRIMARY KEY (`idorganization`, `uuid`),
  UNIQUE INDEX `idorganization_UNIQUE` (`idorganization` ASC),
  UNIQUE INDEX `uuid_UNIQUE` (`uuid` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COMMENT = 'organization tree ';

ALTER TABLE `organization`.`organization` 
CHANGE COLUMN `remark` `remark` TEXT NULL DEFAULT NULL COMMENT 'Describe organizational relationships' ,
ADD COLUMN `relationship` VARCHAR(45) NULL AFTER `remark`;

ALTER TABLE `organization`.`organization` 
ADD COLUMN `child` VARCHAR(45) NULL COMMENT 'child link child[0] to child[n]' AFTER `parents`;


