import React, { Children } from 'react';
import dayjs, { Dayjs } from 'dayjs';
import dayjsGenerateConfig from 'rc-picker/lib/generate/dayjs';
import generatePicker from 'antd/es/date-picker/generatePicker';
import { Space } from 'antd';
const DatePicker = generatePicker<typeof dayjs>(dayjsGenerateConfig as any);

export type Shift = {
	date: Date;
	hourFrom: number;
	hourTo: number;
};

const hoursMin = 9;
const hoursMax = 22;

export const Calendar: React.FC<{
	editMode: boolean;
	header?: React.ReactNode;
	date: Dayjs;
	onDateChange: (date: Dayjs) => void;
	shifts: Shift[];
	onShiftChange: (shifts: ((shifts: any) => Shift[]) | Shift[]) => void;
}> = ({ editMode, header, shifts, onShiftChange, date, onDateChange }) => {
	const calendarRef = React.useRef<HTMLDivElement>(null);
	const [editingShift, setEditingShift] = React.useState<null | {
		index: number;
		init: { cursorY: number; hourFrom: number; hourTo: number };
		editingProperty: 'hourFrom' | 'hourTo';
	}>(null);
	const createShiftMouseDownHandler = React.useCallback(
		(shiftIndex: number) => (event: React.MouseEvent) => {
			if (!calendarRef.current) return;
			if (!editMode) return;
			const { y: calendarY } = calendarRef.current.getBoundingClientRect();
			const shift = shifts[shiftIndex];
			const shiftY = calendarY + 50 + (shift.hourFrom - hoursMin) * 50;
			const shiftHeight = (shift.hourTo - shift.hourFrom) * 50;
			const shiftCursorRelativeY = event.clientY - shiftY;
			const editingProperty = shiftCursorRelativeY / shiftHeight < 0.5 ? 'hourFrom' : 'hourTo';
			event.preventDefault();
			setEditingShift({
				index: shiftIndex,
				init: {
					cursorY: event.clientY,
					hourFrom: shifts[shiftIndex].hourFrom,
					hourTo: shifts[shiftIndex].hourTo,
				},
				editingProperty,
			});
		},
		[shifts, editMode],
	);
	const createCellMouseDownHandler = React.useCallback(
		(date: Date) => (event: React.MouseEvent) => {
			if (!calendarRef.current) return;
			if (!editMode) return;
			event.preventDefault();
			const { y: calendarY } = calendarRef.current.getBoundingClientRect();
			const hourFrom = Math.round(((event.clientY - calendarY - 50) / 50 + hoursMin) / 0.5) * 0.5;
			const hourTo = hourFrom;
			const shiftIndex = shifts.length;

			onShiftChange((shifts): Shift[] => [...shifts, { date, hourFrom, hourTo }]);
			setEditingShift({
				index: shiftIndex,
				init: {
					cursorY: event.clientY,
					hourFrom,
					hourTo,
				},
				editingProperty: 'hourTo',
			});
		},
		[shifts, editMode],
	);
	const handleMouseMove = React.useCallback(
		(event: React.MouseEvent) => {
			if (!editingShift) return;
			if (!editMode) return;

			event.preventDefault();

			const { clientY } = event;
			const { editingProperty } = editingShift;
			const yDiff = clientY - editingShift.init.cursorY;
			const prevShifts = [...shifts];
			const shift = prevShifts[editingShift.index];
			const hours = editingShift.init[editingProperty] + yDiff / 50;
			const rounded = Math.round(hours / 0.5) * 0.5;
			const clamped = Math.min(hoursMax, Math.max(hoursMin, rounded));
			shift[editingProperty] = clamped;
			if (shift.hourFrom >= shift.hourTo) {
				setEditingShift((prevEditingShift: any) => ({
					...prevEditingShift,
					editingProperty: editingProperty === 'hourFrom' ? 'hourTo' : 'hourFrom',
					init: {
						...prevEditingShift.init,
						hourFrom: prevEditingShift.init.hourTo,
						hourTo: prevEditingShift.init.hourFrom,
					},
				}));
			}
			onShiftChange(prevShifts);
		},
		[editingShift, shifts, editMode],
	);
	const handleMouseUp = React.useCallback(() => setEditingShift(null), []);

	React.useEffect(() => {
		if (editingShift) return;
		if (!editMode) return;
		const updatedShifts: typeof shifts = [...shifts];
		let needUpdate = false;
		const colisionMap: { [day: number]: { [hour: number]: typeof updatedShifts[0] } } = {};
		for (let i = 0; i < updatedShifts.length; i++) {
			const shift = updatedShifts[i];
			if (shift.hourFrom >= shift.hourTo) {
				updatedShifts[i] = undefined as any;
				needUpdate = true;
			}
			const day =
				Math.round(
					dayjs(shift.date).startOf('date').unix() - dayjs(shift.date).startOf('week').unix(),
				) /
				(60 * 60 * 24);
			for (
				let time = Math.round(shift.hourFrom * 10);
				time <= Math.round(shift.hourTo * 10);
				time += 5
			) {
				if (!colisionMap[day]) colisionMap[day] = {};
				if (colisionMap[day][time]) {
					colisionMap[day][time].hourFrom = Math.min(
						colisionMap[day][time].hourFrom,
						shift.hourFrom,
					);
					colisionMap[day][time].hourTo = Math.max(colisionMap[day][time].hourTo, shift.hourTo);
					updatedShifts[i] = undefined as any;
					needUpdate = true;
				} else {
					colisionMap[day][time] = shift;
				}
			}
		}
		console.log(colisionMap);
		if (needUpdate) {
			onShiftChange(updatedShifts.filter((shift) => shift !== undefined));
		}
	}, [shifts, editingShift, editMode]);
	const createShiftDoubleClickHandler = React.useCallback(
		(shfitIndex: number) => () => {
			if (!editMode) return;
			onShiftChange((prevShifts) => {
				prevShifts.splice(shfitIndex, 1);
				return [...prevShifts];
			});
		},
		[editMode],
	);

	return (
		<div>
			<Space>
				<DatePicker value={date as any} onChange={onDateChange as any} allowClear={false} />
				{header}
			</Space>
			<br />
			<br />
			<div
				className="calendar"
				onMouseMove={handleMouseMove}
				onMouseUp={handleMouseUp}
				ref={calendarRef}
				style={{ height: (hoursMax - hoursMin + 1) * 50 }}
			>
				<div className="calendar-header calendar-row">
					<div className="calendar-cell">Time/Date</div>
					{Array(7)
						.fill(0)
						.map((_, dayOffset) => (
							<div key={dayOffset} className="calendar-date calendar-cell">
								{new Intl.DateTimeFormat('en-US', {
									day: '2-digit',
									month: 'short',
									weekday: 'short',
								}).format(date.startOf('week').unix() * 1000 + dayOffset * 1000 * 60 * 60 * 24)}
							</div>
						))}
				</div>
				{Array(hoursMax - hoursMin)
					.fill(0)
					.map((_, hourOffset) => (
						<div className="calendar-row" key={hourOffset}>
							<div className="calendar-cell">{hoursMin + hourOffset}:00</div>
							{Array(7)
								.fill(0)
								.map((_, dayOffset) => (
									<div
										key={dayOffset}
										className={editMode ? `calendar-cell calendar-cell-editable` : `calendar-cell`}
										onMouseDown={createCellMouseDownHandler(
											new Date(
												date.startOf('week').add(-1, 'days').unix() * 1000 +
													(dayOffset + 1) * 1000 * 60 * 60 * 24,
											),
										)}
									/>
								))}
						</div>
					))}
				{shifts.map((shift, index) => (
					<div
						key={`${shift.date.toISOString()}-${shift.hourFrom}-${shift.hourTo}`}
						className={editMode ? 'calendar-shift calendar-shift-editable' : 'calendar-shift'}
						onMouseDown={createShiftMouseDownHandler(index)}
						onDoubleClick={createShiftDoubleClickHandler(index)}
						style={{
							top: 50 + (shift.hourFrom - hoursMin) * 50,
							height: (shift.hourTo - shift.hourFrom) * 50,
							left:
								100 *
									((dayjs(shift.date).startOf('date').add(1, 'days').unix() -
										dayjs(shift.date).startOf('week').unix()) /
										(60 * 60 * 24)) -
								2,
						}}
					/>
				))}
			</div>
		</div>
	);
};
