import React, { useState } from 'react';
import { useGetDishes, useWhoAmI } from '../api/dinerComponents';
import { Dishes } from './Dishes';
import { Dish } from '../api/dinerSchemas';
import { SearchByName } from './SearchByName';
import { Button, message } from 'antd';

export const DishesWithSearch: React.FC = () => {
	const [searchQuery, setSearchQuery] = useState('');
	const dishes = useGetDishes({
		queryParams: {
			name: searchQuery,
		},
	});
	const whoAmI = useWhoAmI({});
	const onSearchQueryChange = (value: string) => setSearchQuery(value);
	return (
		<div>
			<SearchByName onChange={onSearchQueryChange} placeholder={'Dishes search'}></SearchByName>
			<br />
			<Button
				hidden={!(whoAmI.data?.role === 3)}
				onClick={() => message.warn('Not implemented yet :)')}
			>
				Add dish
			</Button>
			<Dishes dishes={dishes.data as Dish[]} isLoading={dishes.isLoading}></Dishes>
		</div>
	);
};
