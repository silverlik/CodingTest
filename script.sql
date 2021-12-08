
use [D:\REPOS\CODINGTEST\DBCODINGTEST.MDF]
--#3.a Write a query to return the total expense in salary given to employee working for the RnD
--and Finance department.
select sum(salary) As 'Total Expense in Salary' from dbo.Employee 
inner join dbo.Department
on Employee.DepartmentId = Department.Id
where Department.Name in ('RnD','Finance')

--Result:
--Total Expense in Salary
--495223




--#3.b Write a query to return the first name, the last name and the total bonus given to each
--employee. The bonus, if you are eligible, is calculated like this:
--• 5000 if your salary is below 50000$
--• 10000 if your salary is between 50000 and 85000$
--• 15000 if your salary is over 85000$


select firstname, lastname,
			 CASE WHEN Salary<50000 THEN 5000
				  when salary<85000 then 10000
				  else 15000
			 END  as Bonus
from Employee
inner join Department on  Employee.DepartmentId = Department.Id
where department.EligibleForBonus = 1

--firstname	lastname	Bonus
--Albert	Lupus	15000
--Ginette	Gerard	5000
--Roger	Lavender	10000
--Guy	Lepoint	10000
--Frank	Thestar	15000

--#3.c Write a query to return the names of employee who earn the highest salary
select  Firstname,Lastname, Salary from Employee where Salary = (select max(salary) from employee)

--Firstname	Lastname	Salary
--Jane	Doe	250000
--Frank	Thestar	250000
