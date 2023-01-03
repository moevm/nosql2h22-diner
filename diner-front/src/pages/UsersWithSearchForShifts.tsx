import React, { useState } from 'react';
import { useGetUsers } from '../api/dinerComponents';
import UserList from './Users';
import { User } from '../api/dinerSchemas';
import dayjs from 'dayjs';
import { SearchByNameOrDate } from './SearchByNameOrDate';

export const UserListWithSearchAndTimePicker: React.FC = () => {
	const [searchQuery, setSearchQuery] = useState('');
	const [dateQuery, setDateQuery] = useState(null);
	const [timeQuery, setTimeQuery] = useState('');
	const [date, setDate] = React.useState(dayjs());
	const users = useGetUsers({
		queryParams: {
			nameOrLogin: searchQuery,
			date: dateQuery ? dayjs(dateQuery).format('ddd, MMM D, YYYY hh:mm A').toString() : '',
			hoursMask: timeQuery ? timeQuery : dateQuery ? '001111111111111111111111111' : '',
		},
	});
	const onSearchQueryChange = (value: string) => setSearchQuery(value);
	const onDateQueryChange = (date: any) => setDateQuery(date);
	const onTimeQueryChange = (value: string) => setTimeQuery(value);
	React.useEffect(() => {
		if (users.isLoading) return;
	}, [users.isLoading]);
	return (
		<div>
			<SearchByNameOrDate
				onChangeSearch={onSearchQueryChange}
				onChangeDate={onDateQueryChange}
				onChangeTime={onTimeQueryChange}
			/>
			<UserList users={users.data as User[]} isLoading={users.isLoading} />
		</div>
	);
};
