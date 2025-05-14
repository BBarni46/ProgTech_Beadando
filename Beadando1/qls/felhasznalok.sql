CREATE TABLE `users`.`felhasználók` (`ID` INT NOT NULL AUTO_INCREMENT , `NÉV` VARCHAR(50) NOT NULL , `JELSZÓ` VARCHAR(20) NOT NULL , `EGYENLEG` INT NOT NULL , PRIMARY KEY (`ID`)) ENGINE = InnoDB; 
INSERT INTO `felhasználók` (`ID`, `NÉV`, `JELSZÓ`, `EGYENLEG`) VALUES ('1', 'Bakos Zoltán', 'cicah', '8200');

INSERT INTO `felhasználók` (`ID`, `NÉV`, `JELSZÓ`, `EGYENLEG`) VALUES ('2', 'Bordák Barnabás', 'proba1123', '300');
