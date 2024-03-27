-- MySQL Script generated by MySQL Workbench
-- Tue Mar 26 21:15:04 2024
-- Model: New Model    Version: 1.0
-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema tourism_db
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema tourism_db
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `tourism_db` DEFAULT CHARACTER SET utf8 ;
USE `tourism_db` ;

-- -----------------------------------------------------
-- Table `tourism_db`.`Country`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourism_db`.`Country` (
  `ISO3166-1` VARCHAR(2) NOT NULL,
  `FullName` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`ISO3166-1`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `tourism_db`.`Region`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourism_db`.`Region` (
  `idRegion` INT NOT NULL AUTO_INCREMENT,
  `FullName` VARCHAR(45) NOT NULL,
  `Country_ISO3166-1` VARCHAR(2) NOT NULL,
  PRIMARY KEY (`idRegion`),
  INDEX `fk_Region_Country_idx` (`Country_ISO3166-1` ASC) VISIBLE,
  CONSTRAINT `fk_Region_Country`
    FOREIGN KEY (`Country_ISO3166-1`)
    REFERENCES `tourism_db`.`Country` (`ISO3166-1`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `tourism_db`.`City`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourism_db`.`City` (
  `idCity` INT NOT NULL AUTO_INCREMENT,
  `FullName` VARCHAR(45) NOT NULL,
  `Region_idRegion` INT NOT NULL,
  PRIMARY KEY (`idCity`),
  INDEX `fk_City_Region1_idx` (`Region_idRegion` ASC) VISIBLE,
  CONSTRAINT `fk_City_Region1`
    FOREIGN KEY (`Region_idRegion`)
    REFERENCES `tourism_db`.`Region` (`idRegion`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `tourism_db`.`Hotel`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourism_db`.`Hotel` (
  `idHotel` INT NOT NULL AUTO_INCREMENT,
  `FullName` VARCHAR(45) NOT NULL,
  `Rating` FLOAT NULL,
  `City_idCity` INT NOT NULL,
  PRIMARY KEY (`idHotel`),
  INDEX `fk_Hotel_City1_idx` (`City_idCity` ASC) VISIBLE,
  CONSTRAINT `fk_Hotel_City1`
    FOREIGN KEY (`City_idCity`)
    REFERENCES `tourism_db`.`City` (`idCity`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `tourism_db`.`Attraction`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourism_db`.`Attraction` (
  `idAttraction` INT NOT NULL AUTO_INCREMENT,
  `FullName` VARCHAR(45) NOT NULL,
  `Rating` FLOAT NULL,
  `City_idCity` INT NOT NULL,
  PRIMARY KEY (`idAttraction`),
  INDEX `fk_Attraction_City1_idx` (`City_idCity` ASC) VISIBLE,
  CONSTRAINT `fk_Attraction_City1`
    FOREIGN KEY (`City_idCity`)
    REFERENCES `tourism_db`.`City` (`idCity`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `tourism_db`.`Peculiarity`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourism_db`.`Peculiarity` (
  `idPeculiarity` INT NOT NULL,
  `Decription` TEXT NOT NULL,
  PRIMARY KEY (`idPeculiarity`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `tourism_db`.`Transport`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourism_db`.`Transport` (
  `idTransport` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idTransport`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `tourism_db`.`Route`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourism_db`.`Route` (
  `idRoute` INT NOT NULL AUTO_INCREMENT,
  `From_idCity` INT NOT NULL,
  `To_idCity` INT NOT NULL,
  `Departure` DATETIME NOT NULL,
  `Arrival` DATETIME NOT NULL,
  `Price` DECIMAL(10,2) NOT NULL,
  `Transport_idTransport` INT NOT NULL,
  PRIMARY KEY (`idRoute`),
  INDEX `fk_Route_City1_idx` (`From_idCity` ASC) VISIBLE,
  INDEX `fk_Route_City2_idx` (`To_idCity` ASC) VISIBLE,
  INDEX `fk_Route_Transport1_idx` (`Transport_idTransport` ASC) VISIBLE,
  CONSTRAINT `fk_Route_City1`
    FOREIGN KEY (`From_idCity`)
    REFERENCES `tourism_db`.`City` (`idCity`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Route_City2`
    FOREIGN KEY (`To_idCity`)
    REFERENCES `tourism_db`.`City` (`idCity`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Route_Transport1`
    FOREIGN KEY (`Transport_idTransport`)
    REFERENCES `tourism_db`.`Transport` (`idTransport`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `tourism_db`.`UserRole`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourism_db`.`UserRole` (
  `idUserRole` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idUserRole`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `tourism_db`.`User`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourism_db`.`User` (
  `idUser` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(45) NOT NULL,
  `Surname` VARCHAR(45) NOT NULL,
  `Patronymic` VARCHAR(45) NULL,
  `Birth` DATE NOT NULL,
  `UserRole_idUserRole` INT NOT NULL,
  `Login` VARCHAR(45) NULL,
  `PasswordSHA256` VARCHAR(32) NULL,
  PRIMARY KEY (`idUser`),
  INDEX `fk_User_UserRole1_idx` (`UserRole_idUserRole` ASC) VISIBLE,
  CONSTRAINT `fk_User_UserRole1`
    FOREIGN KEY (`UserRole_idUserRole`)
    REFERENCES `tourism_db`.`UserRole` (`idUserRole`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `tourism_db`.`Entry`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourism_db`.`Entry` (
  `idEntry` INT NOT NULL AUTO_INCREMENT,
  `User_idUser` INT NOT NULL,
  `Route_idRoute` INT NOT NULL,
  PRIMARY KEY (`idEntry`),
  INDEX `fk_Entry_User1_idx` (`User_idUser` ASC) VISIBLE,
  INDEX `fk_Entry_Route1_idx` (`Route_idRoute` ASC) VISIBLE,
  CONSTRAINT `fk_Entry_User1`
    FOREIGN KEY (`User_idUser`)
    REFERENCES `tourism_db`.`User` (`idUser`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Entry_Route1`
    FOREIGN KEY (`Route_idRoute`)
    REFERENCES `tourism_db`.`Route` (`idRoute`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `tourism_db`.`UserTour`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourism_db`.`UserTour` (
  `idUserTour` INT NOT NULL AUTO_INCREMENT,
  `Rating` FLOAT NOT NULL,
  `Arrival_idEntry` INT NOT NULL,
  `Departure_idEntry` INT NOT NULL,
  `Hotel_idHotel` INT NOT NULL,
  PRIMARY KEY (`idUserTour`),
  INDEX `fk_UserTour_Entry1_idx` (`Arrival_idEntry` ASC) VISIBLE,
  INDEX `fk_UserTour_Entry2_idx` (`Departure_idEntry` ASC) VISIBLE,
  INDEX `fk_UserTour_Hotel1_idx` (`Hotel_idHotel` ASC) VISIBLE,
  CONSTRAINT `fk_UserTour_Entry1`
    FOREIGN KEY (`Arrival_idEntry`)
    REFERENCES `tourism_db`.`Entry` (`idEntry`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_UserTour_Entry2`
    FOREIGN KEY (`Departure_idEntry`)
    REFERENCES `tourism_db`.`Entry` (`idEntry`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_UserTour_Hotel1`
    FOREIGN KEY (`Hotel_idHotel`)
    REFERENCES `tourism_db`.`Hotel` (`idHotel`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `tourism_db`.`UserTour_Attraction`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourism_db`.`UserTour_Attraction` (
  `idUserTour_Attraction` INT NOT NULL AUTO_INCREMENT,
  `UserTour_idUserTour` INT NOT NULL,
  `Attraction_idAttraction` INT NOT NULL,
  PRIMARY KEY (`idUserTour_Attraction`),
  INDEX `fk_UserTour_Attraction_UserTour1_idx` (`UserTour_idUserTour` ASC) VISIBLE,
  INDEX `fk_UserTour_Attraction_Attraction1_idx` (`Attraction_idAttraction` ASC) VISIBLE,
  CONSTRAINT `fk_UserTour_Attraction_UserTour1`
    FOREIGN KEY (`UserTour_idUserTour`)
    REFERENCES `tourism_db`.`UserTour` (`idUserTour`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_UserTour_Attraction_Attraction1`
    FOREIGN KEY (`Attraction_idAttraction`)
    REFERENCES `tourism_db`.`Attraction` (`idAttraction`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `tourism_db`.`Pecularities_Countries`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourism_db`.`Pecularities_Countries` (
  `Country_ISO3166-1` VARCHAR(2) NOT NULL,
  `Peculiarity_idPeculiarity` INT NOT NULL,
  `idCountryPecularity` INT NOT NULL AUTO_INCREMENT,
  INDEX `fk_Pecularities_Countries_Country1_idx` (`Country_ISO3166-1` ASC) VISIBLE,
  INDEX `fk_Pecularities_Countries_Peculiarity1_idx` (`Peculiarity_idPeculiarity` ASC) VISIBLE,
  PRIMARY KEY (`idCountryPecularity`),
  CONSTRAINT `fk_Pecularities_Countries_Country1`
    FOREIGN KEY (`Country_ISO3166-1`)
    REFERENCES `tourism_db`.`Country` (`ISO3166-1`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Pecularities_Countries_Peculiarity1`
    FOREIGN KEY (`Peculiarity_idPeculiarity`)
    REFERENCES `tourism_db`.`Peculiarity` (`idPeculiarity`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `tourism_db`.`Pecularities_Regions`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tourism_db`.`Pecularities_Regions` (
  `Region_idRegion` INT NOT NULL,
  `Peculiarity_idPeculiarity` INT NOT NULL,
  `idRegionPecularity` INT NOT NULL AUTO_INCREMENT,
  INDEX `fk_Pecularities_Regions_Region1_idx` (`Region_idRegion` ASC) VISIBLE,
  INDEX `fk_Pecularities_Regions_Peculiarity1_idx` (`Peculiarity_idPeculiarity` ASC) VISIBLE,
  PRIMARY KEY (`idRegionPecularity`),
  CONSTRAINT `fk_Pecularities_Regions_Region1`
    FOREIGN KEY (`Region_idRegion`)
    REFERENCES `tourism_db`.`Region` (`idRegion`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Pecularities_Regions_Peculiarity1`
    FOREIGN KEY (`Peculiarity_idPeculiarity`)
    REFERENCES `tourism_db`.`Peculiarity` (`idPeculiarity`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

USE `tourism_db`;

DELIMITER $$
USE `tourism_db`$$
CREATE DEFINER = CURRENT_USER TRIGGER `tourism_db`.`Route_BEFORE_INSERT` BEFORE INSERT ON `Route` FOR EACH ROW
BEGIN
	IF (YEAR(NEW.Departure) < 2023 OR YEAR(NEW.Arrival) < 2023) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Only routes from 2023 or later is accepted';
    END IF;
    
    IF (NEW.Departure > NEW.Arrival) THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Arrival should be later than departure';
    END IF;
END$$

USE `tourism_db`$$
CREATE DEFINER = CURRENT_USER TRIGGER `tourism_db`.`Route_BEFORE_UPDATE` BEFORE UPDATE ON `Route` FOR EACH ROW
BEGIN
	IF (YEAR(NEW.Departure) < 2023 OR YEAR(NEW.Arrival) < 2023) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Only routes from 2023 or later is accepted';
    END IF;
    
    IF (NEW.Departure > NEW.Arrival) THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Arrival should be later than departure';
    END IF;
END$$

USE `tourism_db`$$
CREATE DEFINER = CURRENT_USER TRIGGER `tourism_db`.`User_BEFORE_INSERT` BEFORE INSERT ON `User` FOR EACH ROW
BEGIN
	IF (TIMESTAMPDIFF(YEAR, NEW.Birth, CURDATE()) < 18) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Age must be at least 18';
    END IF;
END$$

USE `tourism_db`$$
CREATE DEFINER = CURRENT_USER TRIGGER `tourism_db`.`User_BEFORE_UPDATE` BEFORE UPDATE ON `User` FOR EACH ROW
BEGIN
	IF (TIMESTAMPDIFF(YEAR, NEW.Birth, CURDATE()) < 18) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Age must be at least 18';
    END IF;
END$$

USE `tourism_db`$$
CREATE DEFINER = CURRENT_USER TRIGGER `tourism_db`.`UserTour_BEFORE_INSERT` BEFORE INSERT ON `UserTour` FOR EACH ROW
BEGIN
	DECLARE arrivalUser INT;
    DECLARE departureUser INT;
    SET arrivalUser = (SELECT User_idUser FROM `tourism_db`.Entry WHERE idEntry = NEW.Arrival_idEntry);
	SET departureUser = (SELECT User_idUser FROM `tourism_db`.Entry WHERE idEntry = NEW.Departure_idEntry);
	IF (arrivalUser != departureUser) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Users in arrival/departure should be the same';
    END IF;
END$$

USE `tourism_db`$$
CREATE DEFINER = CURRENT_USER TRIGGER `tourism_db`.`UserTour_BEFORE_UPDATE` BEFORE UPDATE ON `UserTour` FOR EACH ROW
BEGIN
	DECLARE arrivalUser INT;
    DECLARE departureUser INT;
    SET arrivalUser = (SELECT User_idUser FROM `tourism_db`.Entry WHERE idEntry = NEW.Arrival_idEntry);
	SET departureUser = (SELECT User_idUser FROM `tourism_db`.Entry WHERE idEntry = NEW.Departure_idEntry);
	IF (arrivalUser != departureUser) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Users in arrival/departure should be the same';
    END IF;
END$$


DELIMITER ;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;