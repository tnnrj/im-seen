-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema MentalAlertDB
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema MentalAlertDB
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `MentalAlertDB` DEFAULT CHARACTER SET utf8mb4 ;
USE `MentalAlertDB` ;

-- -----------------------------------------------------
-- Table `MentalAlertDB`.`Users`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `MentalAlertDB`.`Users` (
  `userID` INT NOT NULL DEFAULT 000000,
  `userName` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`userID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `MentalAlertDB`.`Students`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `MentalAlertDB`.`Students` (
  `studentID` INT NOT NULL AUTO_INCREMENT,
  `studentName` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`studentID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `MentalAlertDB`.`Reports`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `MentalAlertDB`.`Reports` (
  `reportID` INT NOT NULL AUTO_INCREMENT,
  `userID` INT NOT NULL,
  `studentID` INT NOT NULL,
  `studentName` VARCHAR(45) NULL,
  `description` VARCHAR(500) NOT NULL,
  `severity` INT NOT NULL,
  `date` DATETIME NOT NULL,
  PRIMARY KEY (`reportID`),
  INDEX `userID_idx` (`userID` ASC) VISIBLE,
  INDEX `studentID_idx` (`studentID` ASC) VISIBLE,
  CONSTRAINT `userID`
    FOREIGN KEY (`userID`)
    REFERENCES `MentalAlertDB`.`Users` (`userID`)
    ON DELETE NO ACTION
    ON UPDATE CASCADE,
  CONSTRAINT `studentID`
    FOREIGN KEY (`studentID`)
    REFERENCES `MentalAlertDB`.`Students` (`studentID`)
    ON DELETE NO ACTION
    ON UPDATE CASCADE)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
