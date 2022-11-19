import React, { useState } from 'react';
import { useGetDishes } from '../api/dinerComponents';
import { Dishes } from './Dishes';
import { Dish } from '../api/dinerSchemas';
import { SearchByName } from './SearchByName';

export const DishesWithSearch: React.FC = () => {
  const [searchQuery, setSearchQuery] = useState('');
  const dishes = useGetDishes({
    queryParams: {
      name: searchQuery,
    }
  });
  const onSearchQueryChange = (value: string) => setSearchQuery(value);
  return (
    <div>
      <SearchByName onChange={onSearchQueryChange} placeholder={"Dishes search"}></SearchByName>
      <Dishes dishes={dishes.data as Dish[]} isLoading={dishes.isLoading}></Dishes>
    </div>
  )
}
