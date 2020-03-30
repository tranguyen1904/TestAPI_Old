

insert into customer values (100, 'Trang Uyen','M', 033424234, 'HCM');
insert into customer values (101, 'Cus 1','F', 093427584, 'HN');
insert into customer values (102, 'Cus 2','M', 033424234, 'HCM');
insert into customer values (103, 'Cus 3','F', 033424234, 'DN');

insert into employee values (201, 'Emp 1', 'F', 0988526019, 1231231);
insert into employee values (202, 'Emp 2', 'M', 0988526019, 5231231);
insert into employee values (203, 'Emp 3', 'F', 0988526019, 7231231);

insert into purchaseorder values (301, 100, 201, '20191212 12:12:12');
insert into purchaseorder values (302, 101, 201, '20200101 23:12:10');
insert into purchaseorder values (303, 102, 203, getdate());

insert  into product values (401, 'FAMAS Gun', 123415223);
insert  into product values (402, 'Mk 13 Gun', 123415223);
insert  into product values (403, 'M100 Gun', 34415223);
insert  into product values (404, 'AKM Gun', 6533415223);

insert into orderdetail values (1, 301, 401, 10);
insert into orderdetail values (2, 302, 401, 10);
insert into orderdetail values (3, 303, 402, 10);
insert into orderdetail values (4, 301, 401, 10);
insert into orderdetail values (5, 302, 404, 10);