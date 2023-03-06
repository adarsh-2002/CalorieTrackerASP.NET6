

insert into Users values('admin','admin@gmail.com','password','02-14-2002',170,65,2000,'male')
select * from users;
truncate table FoodEntries
truncate table workouts
truncate table users

insert into FoodEntries values(2, 200, '03-05-2023', 100, 23, 200, 10)
insert into FoodEntries values(2, 200, '03-06-2023', 100, 23, 200, 10)
insert into FoodEntries values(2, 200, '03-07-2023', 100, 23, 200, 10)
insert into FoodEntries values(2, 200, '03-05-2023', 100, 23, 200, 10)
insert into FoodEntries values(3, 200, '03-05-2023', 100, 23, 200, 10)

select * from FoodEntries

select * from Workouts

select fe.date, sum(fe.Calories) 'Total calories', sum(fe.Proteins)'Total proteins', sum(fe.Carbs)'Total carbs', sum(fe.Fats)'Total fats' from FoodEntries fe group by Date order by Date

delete from Users where id = 12