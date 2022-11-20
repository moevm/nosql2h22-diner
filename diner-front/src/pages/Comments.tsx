import React from 'react';
import { Avatar, Button, Input, Space, Comment, message } from 'antd';
import { Comment as UserComment } from '../api/dinerSchemas';
import { useCreateComment, useWhoAmI } from '../api/dinerComponents';

export const Comments: React.FC<{
	entity: string;
	entityId: string | number;
	comments: UserComment[];
}> = ({ entity, entityId, comments }) => {
	const [newComment, setNewComment] = React.useState<string | null>(null);
	const createComment = useCreateComment();
	const whoAmI = useWhoAmI({});
	React.useEffect(() => {}, [comments]);
	return (
		<>
			{newComment === null ? (
				<Button
					onClick={() => {
						setNewComment('');
					}}
				>
					Add comment
				</Button>
			) : (
				<div>
					<Space direction="vertical" style={{ width: 500 }}>
						<Input.TextArea
							value={newComment}
							onChange={(event) => setNewComment(event.target.value)}
						/>
						<Space>
							<Button onClick={() => setNewComment(null)}>Discard</Button>
							<Button
								type="primary"
								disabled={!newComment}
								onClick={() => {
									createComment
										.mutateAsync({
											body: {
												resourceId: (entity === 'resource' ? entityId : undefined) as string,
												dishId: (entity === 'dish' ? entityId : undefined) as string,
												userId: whoAmI.data?.id,
												content: newComment,
											},
										})
										.then((res) => {
											message.success('Comment added!');
											comments.push(res as unknown as UserComment);
										})
										.catch((err) => {
											message.error(err.payload);
										});
								}}
							>
								Send comment
							</Button>
						</Space>
					</Space>
				</div>
			)}
			<div style={{ overflow: 'scroll', height: '500px' }}>
				{comments?.map((comment) => (
					<Comment
						key={comment.id}
						author={comment.user?.fullName}
						avatar={
							<Avatar
								src={'https://joeschmoe.io/api/v1/random'}
								alt={comment.user?.fullName ?? 'kek'}
							/>
						}
						content={<p>{comment.content}</p>}
					/>
				))}
			</div>
		</>
	);
};
