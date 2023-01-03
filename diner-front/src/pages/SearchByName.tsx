import { Button, DatePicker, Input } from 'antd';
import React, { useState } from 'react';
import Search from 'antd/es/input/Search';

export const SearchByName: React.FC<{ onChange: (value: string) => void; placeholder: string }> = ({
	onChange,
	placeholder,
}) => {
	const [searchQuery, setSearchQuery] = useState('');
	const onSearch = React.useCallback((value: string) => {
		onChange(value);
	}, []);
	return (
		<div>
			<Input.Group compact size="large">
				<Search
					size={'large'}
					onSearch={onSearch}
					enterButton={'Search'}
					style={{ width: 'calc(100% - 350px)' }}
					placeholder={placeholder}
				/>
			</Input.Group>
		</div>
	);
};
