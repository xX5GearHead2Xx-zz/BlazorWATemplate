Create database BlazorTemplate

create table [User]
(
	User_ID int identity(1,1) primary key not null,
	Email varchar(255) not null,
	Name varchar(100),
	Surname varchar(100),
	Password varbinary(1000) not null,
	Salt varbinary(1000),
	CreateDate datetime not null default GETDATE(),
	LastActive datetime not null default GETDATE()
)

create table ReferenceUserAccessType
(
	AccessType_ID int primary key not null,
	Description varchar(50) not null
)

insert into ReferenceUserAccessType values
(1, 'Edit'),
(2, 'ReadOnly')

create table ReferenceUserRole
(
	Role_ID int primary key not null,
	Description varchar(50)
)

insert into ReferenceUserRole values
(1, 'Developer'),
(2, 'Order Manager'),
(3, 'Product Manager'),
(4, 'Client Manager'),
(5, 'Department Manager'),
(6, 'Supplier Manager'),
(7, 'Accounting Manager')

create table LinkUserRole
(
	LinkUserRole_ID int identity(1,1) primary key not null,
	User_ID int foreign key references [User](User_ID) not null,
	Role_ID int foreign key references ReferenceUserRole(Role_ID) not null,
	AccessType_ID int foreign key references ReferenceUserAccessType(AccessType_ID) not null
)

create table PasswordRecovery
(
	PasswordRecovery_ID int identity(1,1) primary key not null,
	User_ID int foreign key references [User](User_ID) not null,
	Date DateTime not null,
	Expiry DateTime not null
)

