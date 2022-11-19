import React, { useState } from 'react';
import { useGetDishes, useGetResources } from '../api/dinerComponents';
import { Dishes } from './Dishes';
import { Dish } from '../api/dinerSchemas';
import { SearchByName } from './SearchByName';
import { Resources } from './Resources';
import { Button, Space } from 'antd';
import { Link } from 'react-router-dom';

export const ResourcesWithSearch: React.FC = () => {
  const [searchQuery, setSearchQuery] = useState('');
  const dishes = useGetResources({
    queryParams: {
      name: searchQuery,
    }
  });
  const onSearchQueryChange = (value: string) => setSearchQuery(value);
  return (
    <div>
      <SearchByName onChange={onSearchQueryChange} placeholder={"Dishes search"}></SearchByName>
      <br />
      <Space>
        <Link to="/dashboard/resources/add">
          <Button>Add resource</Button>
        </Link>
        <Button>Import</Button>
        <Button>Export</Button>
      </Space>
      <Resources resources={dishes.data as Dish[]} isLoading={dishes.isLoading}></Resources>
    </div>
  )
}
