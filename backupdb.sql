CREATE DATABASE  IF NOT EXISTS `ged` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `ged`;
-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: 50.62.209.147    Database: ged
-- ------------------------------------------------------
-- Server version	5.5.43-37.2-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `TbDepto`
--

DROP TABLE IF EXISTS `TbDepto`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `TbDepto` (
  `cdDepto` int(11) NOT NULL AUTO_INCREMENT,
  `nmDepto` varchar(250) DEFAULT NULL,
  `dtCriacao` datetime DEFAULT NULL,
  `cdentidade` int(11) DEFAULT NULL,
  `tbdeptocol` varchar(45) DEFAULT NULL,
  `isDeletado` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`cdDepto`),
  KEY `fkentidadedepto_idx` (`cdentidade`),
  CONSTRAINT `fkentidadedepto` FOREIGN KEY (`cdentidade`) REFERENCES `TbEntidade` (`cdEntidade`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `TbEntidade`
--

DROP TABLE IF EXISTS `TbEntidade`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `TbEntidade` (
  `cdEntidade` int(11) NOT NULL AUTO_INCREMENT,
  `nmEntidade` varchar(150) DEFAULT NULL,
  `nmUserEntidade` varchar(45) DEFAULT NULL,
  `nmPass` varchar(100) DEFAULT NULL,
  `nmEmail` varchar(200) DEFAULT NULL,
  `cdEntidadeTipo` int(11) NOT NULL,
  `cdEntidadePai` int(11) DEFAULT NULL,
  `isdeletado` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`cdEntidade`,`cdEntidadeTipo`),
  KEY `fk_TbEntidade_tbTipoEntidade_idx` (`cdEntidadeTipo`),
  KEY `fk_TbEntidade_TbEntidade1_idx` (`cdEntidadePai`),
  CONSTRAINT `fk_TbEntidade_TbEntidade1` FOREIGN KEY (`cdEntidadePai`) REFERENCES `TbEntidade` (`cdEntidade`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_TbEntidade_tbTipoEntidade` FOREIGN KEY (`cdEntidadeTipo`) REFERENCES `TbTipoEntidade` (`cdTipoEntidade`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `TbEntidadeDepto`
--

DROP TABLE IF EXISTS `TbEntidadeDepto`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `TbEntidadeDepto` (
  `cdEntidade` int(11) NOT NULL,
  `cdDepto` int(11) NOT NULL,
  PRIMARY KEY (`cdEntidade`,`cdDepto`),
  KEY `fk_tbEntidadeDepto_tbDepto1` (`cdDepto`),
  CONSTRAINT `fk_tbEntidadeDepto_tbDepto1` FOREIGN KEY (`cdDepto`) REFERENCES `TbDepto` (`cdDepto`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbEntidadeDepto_TbEntidade1` FOREIGN KEY (`cdEntidade`) REFERENCES `TbEntidade` (`cdEntidade`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `TbPasta`
--

DROP TABLE IF EXISTS `TbPasta`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `TbPasta` (
  `cdpasta` int(11) NOT NULL AUTO_INCREMENT,
  `nmpasta` varchar(100) DEFAULT NULL,
  `dtcriacao` datetime NOT NULL,
  `dspasta` varchar(500) DEFAULT NULL,
  `cddepto` int(11) DEFAULT NULL,
  `cdpastapai` int(11) DEFAULT NULL,
  `cdentidade` int(11) DEFAULT NULL,
  `isDeletado` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`cdpasta`),
  KEY `fkdepto_idx` (`cddepto`),
  KEY `fkpastapai_idx` (`cdpastapai`),
  KEY `fkentidadepasta_idx` (`cdentidade`),
  CONSTRAINT `fkdepto` FOREIGN KEY (`cddepto`) REFERENCES `TbDepto` (`cdDepto`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fkentidadepasta` FOREIGN KEY (`cdentidade`) REFERENCES `TbEntidade` (`cdEntidade`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fkpastapai` FOREIGN KEY (`cdpastapai`) REFERENCES `TbPasta` (`cdpasta`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=45 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `TbProcesso`
--

DROP TABLE IF EXISTS `TbProcesso`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `TbProcesso` (
  `cdProcesso` int(11) NOT NULL AUTO_INCREMENT,
  `nmProcesso` varchar(200) DEFAULT NULL,
  `dsProcesso` varchar(500) DEFAULT NULL,
  `cdTipoProcesso` int(11) DEFAULT NULL,
  `cdProcessoPai` int(11) DEFAULT NULL,
  `cdpasta` int(11) NOT NULL,
  `cdentidade` int(11) DEFAULT NULL,
  `isDeletado` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`cdProcesso`),
  KEY `fk_tbProcesso_tbTipoProcesso1_idx` (`cdTipoProcesso`),
  KEY `fkpasta_idx` (`cdpasta`),
  KEY `fkentidadeproc_idx` (`cdentidade`),
  CONSTRAINT `fkentidadeproc` FOREIGN KEY (`cdentidade`) REFERENCES `TbEntidade` (`cdEntidade`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fkpasta` FOREIGN KEY (`cdpasta`) REFERENCES `TbPasta` (`cdpasta`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbProcesso_tbTipoProcesso1` FOREIGN KEY (`cdTipoProcesso`) REFERENCES `TbTipoProcesso` (`cdTipoProcesso`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `TbRepositorio`
--

DROP TABLE IF EXISTS `TbRepositorio`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `TbRepositorio` (
  `cdRepositorio` int(11) NOT NULL AUTO_INCREMENT,
  `cdProcesso` int(11) NOT NULL,
  `nm_arquivo` varchar(250) DEFAULT NULL,
  `nm_extensao` varchar(10) DEFAULT NULL,
  `nr_tamanho` float DEFAULT NULL,
  `nm_caminho_disco` varchar(500) DEFAULT NULL,
  `cdentidade` int(11) DEFAULT NULL,
  `isDeletado` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`cdRepositorio`),
  KEY `fk_tbRepositorio_tbLocalProcesso1_idx` (`cdProcesso`),
  KEY `fkentidade_idx` (`cdentidade`),
  CONSTRAINT `fkentidade` FOREIGN KEY (`cdentidade`) REFERENCES `TbEntidade` (`cdEntidade`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_procrep` FOREIGN KEY (`cdProcesso`) REFERENCES `TbProcesso` (`cdProcesso`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `TbTipoEntidade`
--

DROP TABLE IF EXISTS `TbTipoEntidade`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `TbTipoEntidade` (
  `cdTipoEntidade` int(11) NOT NULL AUTO_INCREMENT,
  `nmTipo` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`cdTipoEntidade`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `TbTipoProcesso`
--

DROP TABLE IF EXISTS `TbTipoProcesso`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `TbTipoProcesso` (
  `cdTipoProcesso` int(11) NOT NULL AUTO_INCREMENT,
  `nmTipoProcesso` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`cdTipoProcesso`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping routines for database 'ged'
--
/*!50003 DROP PROCEDURE IF EXISTS `Logar` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = '' */ ;
DELIMITER ;;
CREATE DEFINER=`ged`@`%` PROCEDURE `Logar`(login varchar(10),pass varchar(50))
select * from TbEntidade where nmUserEntidade = login and nmPass = pass 
and (isdeletado= 0 or 
isdeletado is null) ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `pesquisarRepositorioUsuario` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = '' */ ;
DELIMITER ;;
CREATE DEFINER=`ged`@`%` PROCEDURE `pesquisarRepositorioUsuario`(cdEntidade varchar(10),pesq varchar(500))
select rep.*,
pro.nmProcesso,p.nmPasta,d.nmDepto,pro.cdentidade,p.cdPasta 
from TbRepositorio rep
join TbProcesso pro on rep.cdProcesso = pro.cdProcesso
join TbPasta p on p.cdpasta = pro.cdpasta
join TbDepto d on d.cddepto = p.cddepto
join TbEntidadeDepto ed on ed.cdDepto = d.cddepto
where (ed.cdEntidade = cdEntidade)
and (rep.nm_caminho_disco like concat('%',pesq,'%')
or rep.nm_extensao like concat('%',pesq,'%') 
or rep.nm_arquivo like concat('%',pesq,'%') ) ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `RetornarDeptos` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = '' */ ;
DELIMITER ;;
CREATE DEFINER=`ged`@`%` PROCEDURE `RetornarDeptos`()
select d.cdDepto,d.nmDepto,d.dtCriacao,d.cdentidade,e.nmEntidade from TbDepto d
join TbEntidade e on d.cdentidade = e.cdEntidade ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `RetornarDeptosEntidade` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = '' */ ;
DELIMITER ;;
CREATE DEFINER=`ged`@`%` PROCEDURE `RetornarDeptosEntidade`(cdEntidade int)
SELECT ed.cdEntidade,ed.cdDepto,e.nmEntidade,d.nmDepto from TbEntidadeDepto ed 
             join TbEntidade e on ed.cdEntidade = e.cdEntidade 
             join TbDepto d on ed.cdDepto = d.cdDepto where ed.cdEntidade = cdEntidade ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `RetornarProcessosDepto` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = '' */ ;
DELIMITER ;;
CREATE DEFINER=`ged`@`%` PROCEDURE `RetornarProcessosDepto`(cddepto int)
select p.nmProcesso,p.dsProcesso,
d.nmDepto,d.dtCriacao,f.statusDocument from tbfollowup f
join tbdepto d on f.cdDepto = d.cdDepto
join tbprocesso p on p.cdProcesso = f.cdProcesso
where d.cdDepto = 2 ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2017-03-18 14:43:13
