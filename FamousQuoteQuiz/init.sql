create database pollDb collate SQL_Latin1_General_CP1_CI_AS
go

use pollDb
go

create table Author
(
    Id   int identity
        primary key,
    Name varchar(250) not null
)
go

create table Quote
(
    Id       int identity
        primary key,
    Body     varchar(1000) not null,
    AuthorId int          not null
        references Author
)
go

create table [User]
(
    Id           int identity
        primary key,
    Name         varchar(100)               not null,
    QuestionType int                        not null,
    CreatedAt    datetime default getdate() not null
)
go

create table UserAchievement(
    Id int identity primary key,
    UserId int not null foreign key references [User] (Id) on delete cascade,
    QuoteId int not null foreign key references Quote (Id) on delete cascade,
    IsAnsweredCorrectly bit not null
)


insert into [User] (Name, QuestionType) values ('admin', 0)
insert into Author (Name) values ('Dante')
insert into Author (Name) values ('Aram Asatryan')
insert into Author (Name) values ('Virgil')
insert into Quote(Body, AuthorId) values ('I found myself within a forest dark, For the straightforward pathway had been lost.', 1)

select * from Quote
select * from Author
select * from [User]
select * from UserAchievement
