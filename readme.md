# note

## 演示使用数据

### orgs

	org-1
		org-1.1
	org-2
		org-2.1

### users

	user-super	-> org1
	user-admin	-> org2
	user-no-org	-> null
	user-no-org2	-> org-no-exist
	user-001	-> org1.1
	user-002	-> org1.1
	user-003	-> org1.1


### courses

	course-a		-> user-super
	course-b		-> user-admin
	course-001~012	-> user-admin
	course-013~015	-> user-no-org


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