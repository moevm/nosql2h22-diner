import { Button, message, Spin } from 'antd';
import dayjs, { Dayjs } from 'dayjs';
import React from 'react';
import { useGetUser, useGetWeek, useUpdateWeek } from '../api/dinerComponents';
import { Calendar, Shift } from './Calendar';

const DAYS_OF_WEEK = {
	sunday: 0,
	monday: 1,
	tuesday: 2,
	wednesday: 3,
	thursday: 4,
	friday: 5,
	saturday: 6,
};

const weekDay = ['sunday', 'monday', 'tuesday', 'wednesday', 'thursday', 'friday', 'saturday'];

const hourMap = (x: Date) => {
	const day = dayjs(x);
	const map = [
		22, 51, 21, 50, 20, 49, 19, 48, 18, 47, 17, 46, 16, 45, 15, 44, 14, 43, 13, 42, 12, 41, 11, 40,
		10, 39, 9,
	];
	console.log(day.toDate());
	day.set('minutes', day.get('minutes') > 30 ? 30 : 0);
	return map.indexOf(day.get('hours') + day.get('minutes'));
};

export const binaryFromDateTime = (from: Date, to: Date = new Date(0)) => {
	if (to < from || from > to) return 0;
	console.log(from, to);
	const hoursFrom = hourMap(from);
	const hoursTo = hourMap(to);
	console.log(hoursFrom, hoursTo);
	let hours = 0;
	for (let i = hoursTo; i < hoursFrom; ++i) hours |= 1 << i;
	hours |= 1 << hoursFrom;
	return hours;
};

export const dateTimeFromBinary = (binaryDate: number) => {
	const result = [];
	for (let i = 0; i < 28; ++i) {
		if ((binaryDate >> i) & 1) result.push(i);
	}
	const hoursMapped = [
		22, 51, 21, 50, 20, 49, 19, 48, 18, 47, 17, 46, 16, 45, 15, 44, 14, 43, 13, 42, 12, 41, 11, 40,
		10, 39, 9,
	];
	const hours = result.map((x) => hoursMapped[x]);
	const hoursNumbers = hours.map((x) => (x - 30 < 0 ? x : x - 30 + 0.5));
	let i = 0;
	let answers = [];
	for (let l = 1; l < hoursNumbers.length; l++) {
		let diff = hoursNumbers[l - 1] - hoursNumbers[l];
		if (diff > 0.5) {
			answers.push(hoursNumbers[i], hoursNumbers[l - 1]);
			i = l;
		}
		if (l === hoursNumbers.length - 1) answers.push(hoursNumbers[i], hoursNumbers[l]);
	}
	return answers;
};

export const Shifts: React.FC<{ id: string }> = ({ id }) => {
	const [editMode, setEditMode] = React.useState(false);
	const [date, setDate] = React.useState(dayjs());
	const [shifts, setShifts] = React.useState<Shift[]>([]);
	const user = useGetUser({ queryParams: { id } });
	const serverWeek = useGetWeek({ queryParams: { id, dateTime: date.toString() } });
	const updateServerWeek = useUpdateWeek();
	React.useEffect(() => {
		if (serverWeek.data) {
			const result: Shift[] = [];
			for (let key in serverWeek.data as any) {
				if (typeof serverWeek.data[key] === 'number') {
					const day: number[] = dateTimeFromBinary(serverWeek.data[key]);
					const dayPairs: number[][] = day.reduce(function (res: number[][], value, index, array) {
						if (index % 2 === 0) res.push(day.slice(index, index + 2));
						return res;
					}, []);
					result.push(
						...dayPairs.map((x): Shift => {
							return {
								date: dayjs((serverWeek.data as any)?.createdAt)
									.add(Number(DAYS_OF_WEEK[key as keyof typeof DAYS_OF_WEEK]), 'days')
									.startOf('day')
									.toDate(),
								hourFrom: x[1],
								hourTo: x[0],
							};
						}),
					);
				}
			}
			setDate(dayjs((serverWeek.data as any)?.createdAt));
			setShifts(result);
		} else setShifts([]);
	}, [serverWeek.data]);

	if (serverWeek.isLoading) return <Spin />;
	if (user.isLoading) return <Spin />;
	if (serverWeek.error) {
		console.log(serverWeek.error);
		return <div>Error occured</div>;
	}

	return (
		<div>
			<p>{user.data?.fullName}</p>
			<Calendar
				editMode={editMode}
				shifts={shifts}
				onShiftChange={setShifts}
				date={date}
				onDateChange={setDate}
				header={
					<Button
						hidden={
							!(
								user.data?.role === 'Admin' ||
								user.data?.role === 'Manager' ||
								user.data?.id === id
							)
						}
						onClick={() => {
							setEditMode((editMode) => !editMode);
							if (editMode) {
								const weekMap: { [key: string]: any } = {
									sunday: 0,
									monday: 0,
									tuesday: 0,
									wednesday: 0,
									thursday: 0,
									friday: 0,
									saturday: 0,
									createdAt: (serverWeek.data as any)?.createdAt,
									userId: id,
								};
								shifts.map((x) => {
									const date = dayjs(x.date).startOf('day');
									const bin = binaryFromDateTime(
										date.add(x.hourFrom, 'hours').toDate(),
										date.add(x.hourTo, 'hours').toDate(),
									);
									weekMap[weekDay[date.day()]] = weekMap[weekDay[date.day()]] | bin || 0;
									console.log('0' + weekMap[weekDay[date.day()]].toString(2));
									weekMap.createdAt = date.toDate();
								});
								for (let key in weekMap) {
									if (typeof weekMap[key] === 'number')
										weekMap[key] = '0' + (weekMap[key] >> 0).toString(2);
								}
								updateServerWeek.mutateAsync({ body: weekMap }).then((res) => {
									message.success('Shift updated!');
									serverWeek.refetch();
								});
							}
						}}
					>
						{editMode ? 'Save changes' : 'Edit'}
					</Button>
				}
			/>
		</div>
	);
};
