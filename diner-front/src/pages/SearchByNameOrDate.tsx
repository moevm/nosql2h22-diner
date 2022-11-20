import { Button, DatePicker, Input } from 'antd';
import React, { useState } from 'react';
import Search from 'antd/es/input/Search';
import { RangePicker } from 'rc-picker';
import { binaryFromDateTime } from './Shifts';

export const SearchByNameOrDate: React.FC<{
	onChangeSearch: (value: string) => void;
	onChangeDate: (value: Date) => void;
	onChangeTime: (values: string) => void;
}> = ({ onChangeSearch, onChangeDate, onChangeTime }) => {
	const [searchQuery, setSearchQuery] = useState('');
	const onSearch = React.useCallback((value: string) => {
		onChangeSearch(value);
	}, []);
	const onPickDate = React.useCallback((date: any, dateString: any) => {
		console.log(new Date(date));
		onChangeDate(new Date(date));
	}, []);
	const onPickTime = React.useCallback((values: any, dateString: any) => {
		const bin =
			'0' + (binaryFromDateTime(new Date(values[0]), new Date(values[1])) >>> 0).toString(2);
		console.log(bin);
		onChangeTime(bin);
	}, []);
	return (
		<div>
			<Input.Group
				compact
				size="large"
				style={{ display: 'flex', marginRight: '30px', alignItems: 'center', flexDirection: 'row' }}
			>
				<Search
					onSearch={onSearch}
					enterButton={'Search'}
					size={'large'}
					style={{ width: 'calc(100% - 550px)' }}
					placeholder="User search by shift's params"
				/>
				<DatePicker size={'large'} onChange={onPickDate} />
				<DatePicker.RangePicker size={'large'} picker={'time'} onChange={onPickTime} />
			</Input.Group>
		</div>
	);
};
