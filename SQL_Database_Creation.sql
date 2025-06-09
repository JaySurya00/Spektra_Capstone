create database SpektraDB

use SpektraDB;

create table [Events](
id integer primary key,
[date] date not null,
[time] time not null,
[location] varchar(500) not null,
capacity integer,
event_type varchar(200) not null,
ticket_cost integer not null,
organizer varchar(200),
created_at date default getdate()
)

create table Tickets(
id integer primary key,
event_id integer not null,
owner varchar(200) not null,
created_at date default getdate()
)

create table Users(
name varchar(200),
email varchar(200) primary key,
dob date not null,
role varchar(100) not null,
created_at date default getdate()
)

alter table dbo.Events add constraint FK_Event_Organizer foreign key (organizer) references Users(email);
alter table Tickets add constraint FK_Event_Ticket foreign key (event_id) references Events(id);
alter table Tickets add constraint FK_Ticket_Owner foreign key (owner) references Users(email);

alter table Users add password varchar(100) not null;