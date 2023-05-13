-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Versión del servidor:         8.0.31 - MySQL Community Server - GPL
-- SO del servidor:              Win64
-- HeidiSQL Versión:             12.3.0.6589
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Volcando estructura de base de datos para dada
DROP DATABASE IF EXISTS `dada`;
CREATE DATABASE IF NOT EXISTS `dada` /*!40100 DEFAULT CHARACTER SET utf8mb3 */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `dada`;

-- Volcando estructura para tabla dada.customers
DROP TABLE IF EXISTS `customers`;
CREATE TABLE IF NOT EXISTS `customers` (
  `Id` varchar(50) NOT NULL,
  `Document` varchar(50) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `LastName` varchar(100) NOT NULL,
  `Address` varchar(200) DEFAULT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `Phone` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- La exportación de datos fue deseleccionada.

-- Volcando estructura para tabla dada.inventorymovementdetails
DROP TABLE IF EXISTS `inventorymovementdetails`;
CREATE TABLE IF NOT EXISTS `inventorymovementdetails` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `InventoryMovementId` int NOT NULL,
  `ProductId` varchar(50) NOT NULL,
  `Quantity` double NOT NULL,
  `UnitCost` double NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_InventoryMovement_Detail` (`InventoryMovementId`),
  KEY `FK_InventoryMovement_Product` (`ProductId`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;

-- La exportación de datos fue deseleccionada.

-- Volcando estructura para tabla dada.inventorymovements
DROP TABLE IF EXISTS `inventorymovements`;
CREATE TABLE IF NOT EXISTS `inventorymovements` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Type` varchar(50) NOT NULL,
  `Date` date NOT NULL,
  `Description` varchar(200) DEFAULT NULL,
  `Status` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;

-- La exportación de datos fue deseleccionada.

-- Volcando estructura para tabla dada.invoicedetails
DROP TABLE IF EXISTS `invoicedetails`;
CREATE TABLE IF NOT EXISTS `invoicedetails` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `InvoiceId` int NOT NULL,
  `ProductId` varchar(50) NOT NULL,
  `Quantity` double NOT NULL DEFAULT '0',
  `UnitPrice` double NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `FK_Invoices_Details` (`InvoiceId`),
  KEY `FK_Invoices_Products` (`ProductId`),
  CONSTRAINT `FK_Invoices_Details` FOREIGN KEY (`InvoiceId`) REFERENCES `invoices` (`Id`),
  CONSTRAINT `FK_Invoices_Products` FOREIGN KEY (`ProductId`) REFERENCES `products` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;

-- La exportación de datos fue deseleccionada.

-- Volcando estructura para tabla dada.invoices
DROP TABLE IF EXISTS `invoices`;
CREATE TABLE IF NOT EXISTS `invoices` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `CustomerId` varchar(50) NOT NULL,
  `Date` date NOT NULL,
  `Status` int NOT NULL,
  `Description` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_Customer_Invoices` (`CustomerId`),
  CONSTRAINT `FK_Customer_Invoices` FOREIGN KEY (`CustomerId`) REFERENCES `customers` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;

-- La exportación de datos fue deseleccionada.

-- Volcando estructura para tabla dada.products
DROP TABLE IF EXISTS `products`;
CREATE TABLE IF NOT EXISTS `products` (
  `Id` varchar(50) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Description` varchar(200) NOT NULL,
  `Cost` double NOT NULL,
  `Availability` int NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- La exportación de datos fue deseleccionada.

-- Volcando estructura para tabla dada.purchasedetails
DROP TABLE IF EXISTS `purchasedetails`;
CREATE TABLE IF NOT EXISTS `purchasedetails` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `PurchaseId` int NOT NULL,
  `Line` int NOT NULL,
  `ProductId` varchar(50) NOT NULL,
  `Description` varchar(200) DEFAULT NULL,
  `Quantity` double NOT NULL,
  `UnitCost` double NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_Purchases_Detail` (`PurchaseId`),
  KEY `FK_Purchases_Product` (`ProductId`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;

-- La exportación de datos fue deseleccionada.

-- Volcando estructura para tabla dada.purchases
DROP TABLE IF EXISTS `purchases`;
CREATE TABLE IF NOT EXISTS `purchases` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Date` date NOT NULL,
  `Description` varchar(200) DEFAULT NULL,
  `Status` int NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;

-- La exportación de datos fue deseleccionada.

-- Volcando estructura para tabla dada.salesreportdetails
DROP TABLE IF EXISTS `salesreportdetails`;
CREATE TABLE IF NOT EXISTS `salesreportdetails` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SalesReportId` int NOT NULL,
  `Line` int NOT NULL,
  `ProductId` varchar(50) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Quantity` double NOT NULL DEFAULT '0',
  `UnitCost` double NOT NULL DEFAULT '0',
  `UnitPrice` double NOT NULL DEFAULT '0',
  `Description` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_SalesReport_Details` (`SalesReportId`),
  KEY `FK_SalesReportDetail` (`ProductId`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb3;

-- La exportación de datos fue deseleccionada.

-- Volcando estructura para tabla dada.salesreports
DROP TABLE IF EXISTS `salesreports`;
CREATE TABLE IF NOT EXISTS `salesreports` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Date` date NOT NULL,
  `CustomerId` varchar(50) NOT NULL,
  `Description` varchar(200) DEFAULT NULL,
  `Status` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_SalesReport_Customer` (`CustomerId`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb3;

-- La exportación de datos fue deseleccionada.

-- Volcando estructura para tabla dada.workorderdetails
DROP TABLE IF EXISTS `workorderdetails`;
CREATE TABLE IF NOT EXISTS `workorderdetails` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `WorkOrderId` int NOT NULL,
  `ProductId` varchar(50) NOT NULL,
  `Quantity` double NOT NULL,
  `UnitCost` double NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_WorkorderDetail` (`WorkOrderId`),
  KEY `FK_WorkOrderProduct` (`ProductId`),
  CONSTRAINT `FK_WorkorderDetail` FOREIGN KEY (`WorkOrderId`) REFERENCES `workorders` (`Id`),
  CONSTRAINT `FK_WorkOrderProduct` FOREIGN KEY (`ProductId`) REFERENCES `products` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;

-- La exportación de datos fue deseleccionada.

-- Volcando estructura para tabla dada.workorders
DROP TABLE IF EXISTS `workorders`;
CREATE TABLE IF NOT EXISTS `workorders` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `CustomerId` varchar(50) NOT NULL,
  `Date` date NOT NULL,
  `Description` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_WorkOrder_Customer` (`CustomerId`),
  CONSTRAINT `FK_WorkOrder_Customer` FOREIGN KEY (`CustomerId`) REFERENCES `customers` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb3;

-- La exportación de datos fue deseleccionada.

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
