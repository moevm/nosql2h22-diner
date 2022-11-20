import React, { useState } from 'react';
import { Input, DatePicker, Button, List, Slider } from 'antd';
import { Link } from 'react-router-dom';
import { useGetPayments } from '../api/dinerComponents';
import { SearchByName } from './SearchByName';

const PAYMENT_TYPES = ['For Order', 'Lease'];

export const Payments: React.FC = () => {
	const [searchQuery, setSearchQuery] = useState('');
	const [rangeQuery, setRangeQuery] = useState([0, 250]);
	const onSearchQueryChange = (value: string) => setSearchQuery(value);
	const onSliderChanges = (values: [number, number] | number) => {
		setRangeQuery(Array.isArray(values) ? values : [values]);
	};
	const paymentsList = useGetPayments({
		queryParams: { number: searchQuery as unknown as number, gt: rangeQuery[0], lt: rangeQuery[1] },
	});
	return (
		<div>
			<SearchByName placeholder={'Payments search'} onChange={onSearchQueryChange} />
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
