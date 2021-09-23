# note

## 演示使用数据

- org
- user
- course

### orgs

	- org-1
		- user-super
			- course-a
	- org-1.1
		- user-001
		- user-002
		- user-003
	- org-2
		- user-admin
			- course-b
			- course-001
			- course-002
			- ...
			- course-012
	- org-2.1
		- 

### users

	user-super	-> org1
	user-admin	-> org2
	user-001	-> org1.1
	user-002	-> org1.1
	user-003	-> org1.1
	user-no-org	-> null
	user-no-org2-> org-no-exist


### courses

	course-a		-> user-super		->		org1
	course-b		-> user-admin		->		org2
	course-001~012	-> user-admin		->		org1
	course-013~015	-> user-no-org		->		NULL

### demo data

	- org-1
		- user-super
			- course-a
	- org-1.1
		- user-001
		- user-002
		- user-003
	- org-2
		- user-admin
			- course-b
			- course-001
			- course-002
			- ...
			- course-012
	- org-2.1
		- 

### org's user count

- org1.1:	3
- org1:		1
- org2:		1
- org1.2:	0

### user's course count

- user-admin:	13
- user-no-org:	3
- user-super:	1
- user-001:	0
- user-002:	0
- user-003:	0

### org's course count

- org2:		13
- org1:		1
- org1.1:	0
- org1.2:	0