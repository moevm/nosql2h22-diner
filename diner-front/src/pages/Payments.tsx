import React, { useState } from 'react';
import { Button, DatePicker, Input, List, Select, Slider, Space } from 'antd';
import { Link } from 'react-router-dom';
import {useGetPayments, useGetUsers} from '../api/dinerComponents';
import { SearchByName } from './SearchByName';
import dayjs from 'dayjs';

const PAYMENT_TYPES = ['For Order', 'Lease'];

export const Payments: React.FC = () => {
	const [searchQuery, setSearchQuery] = useState('');
	const [rangeQuery, setRangeQuery] = useState([0, 250]);
	const onSearchQueryChange = (value: string) => setSearchQuery(value);
	const onSliderChanges = (values: [number, number] | number) => {
		setRangeQuery(Array.isArray(values) ? values : [values]);
	};
	const [userIdParam, setUserIdParam] = useState('');
	const [dateParam, setDateParam] = useState(null)
	const paymentsList = useGetPayments({
		queryParams: {
			number: searchQuery as unknown as number,
			gt: rangeQuery[0],
			lt: rangeQuery[1],
			userId: userIdParam,
			date: dateParam ? dayjs(dateParam).format('ddd, MMM D, YYYY hh:mm A').toString() : '',
		},
	});
	const [userQuerySearch, setUserQuerySearch] = useState('')
	const users = useGetUsers({
		queryParams: {
			nameOrLogin: userQuerySearch,
		}
	});
	const onUserQueryChange = (val: string) => {
		setUserIdParam(val)
	}
	const onSearch = (val: string) => {
		setUserQuerySearch(val);
	}
	const onPickDate = (val: any) => {
		setDateParam(val)
	}
	return (
		<div>
			<SearchByName placeholder={'Payments search'} onChange={onSearchQueryChange} />
			<Space>
				<DatePicker size={'large'} onChange={onPickDate} />
				<Button onClick={() => onPickDate(null)}> Clear </Button>
			</Space>
			<br />
			<Select
				showSearch
				placeholder="Select a user"
				optionFilterProp="children"
				onChange={onUserQueryChange}
				onSearch={onSearch}
				filterOption={(input, option) =>
					(option?.label ?? '').toLowerCase().includes(input.toLowerCase())
				}
				options={users.data?.map((x) => {
					return {
						label: x.login,
						value: x.id,
					};
				})}
			/>
			<br />
			<br />
			<p> Price range: </p>
			<br />
			<Slider
				onAfterChange={onSliderChanges}
				range
				defaultValue={[50, 250]}
				max={1000}
				style={{ width: 500 }}
			/>
			<br />
			<br />
			<List
				size="large"
				header={
					<div>Payments found: {paymentsList.data?.length ? paymentsList.data?.length : 0}</div>
				}
				dataSource={paymentsList.data}
				renderItem={(item) => (
					<Link key={item.id} to={`/dashboard/payments/${item.id}`}>
						{' '}
						<List.Item>
							#{item.number} {PAYMENT_TYPES[item.type as number]} <strong>$ {item.price}</strong>
						</List.Item>
					</Link>
				)}
			/>
		</div>
	);
};
