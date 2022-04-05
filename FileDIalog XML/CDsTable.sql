USE `cdstable`;
CREATE TABLE CDs(
                    `id` int NOT NULL AUTO_INCREMENT,
                    `Titel` varchar(255) NOT NULL,
                    `Artist` varchar(255) NULL,
                    `Country` varchar(255) NULL,
                    `Company` varchar(255) NULL,
                    `Price` varchar(255) NULL,
                    `Year` varchar(255) NULL,
                    PRIMARY KEY (id)
);
INSERT CDs (`id`, `Titel`, `Artist`, `Country`, `Company`, `Price`, `Year`) VALUES (1, N'Empire Burlesque', N'Bob Dylan', N'USA', N'Columbia', N'10.90', N'1985');
INSERT CDs (`id`, `Titel`, `Artist`, `Country`, `Company`, `Price`, `Year`) VALUES (2, N'Hide your heart', N'Bonnie Tyler', N'UK', N'CBS Records', N'9.90', N'1988');
INSERT CDs (`id`, `Titel`, `Artist`, `Country`, `Company`, `Price`, `Year`) VALUES (3, N'Loose Yourself', N'Eminem', N'USA', N'Marshal Matters', N'10.90', N'1999');
INSERT CDs (`id`, `Titel`, `Artist`, `Country`, `Company`, `Price`, `Year`) VALUES (4, N'Hearts Burst Into Fire', N'Bullet for my Valentine', N'UK', N'CBS Records', N'6.66', N'2006');
INSERT CDs (`id`, `Titel`, `Artist`, `Country`, `Company`, `Price`, `Year`) VALUES (5, N'Break Stuff', N'Limp Bizkit', N'USA', N'Bizkits', N'10.90', N'2003');
INSERT CDs (`id`, `Titel`, `Artist`, `Country`, `Company`, `Price`, `Year`) VALUES (6, N'Situations', N'Escape the Fate', N'USA', N'Unknown', N'9.90', N'2006')