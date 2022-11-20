/**
 * Generated by @openapi-codegen
 *
 * @version v1
 */
export type AuthDto = {
	login?: string | null;
	password?: string | null;
};

export type Comment = {
	id?: string | null;
	/**
	 * @format date-time
	 */
	createdAt?: string;
	/**
	 * @format date-time
	 */
	updatedAt?: string;
	content?: string | null;
	userId?: string | null;
	user?: User;
	dishId?: string | null;
	dish?: Dish;
	resourceId?: string | null;
	resource?: Resource;
};

export type CreateUserDto = {
	fullName?: string | null;
	login?: string | null;
	role?: UserRole;
	password?: string | null;
};

export type Dish = {
	id?: string | null;
	/**
	 * @format date-time
	 */
	createdAt?: string;
	/**
	 * @format date-time
	 */
	updatedAt?: string;
	name?: string | null;
	description?: string | null;
	/**
	 * @format int32
	 */
	price?: number;
	comments?: string[] | null;
	commentsList?: Comment[] | null;
};

export type DishDto = {
	id?: string | null;
	name?: string | null;
	description?: string | null;
	/**
	 * @format int32
	 */
	price?: number;
	listDishResourceDtos?: DishResourceDto[] | null;
};

export type DishResourceDto = {
	id?: string | null;
	resourceId?: string | null;
	/**
	 * @format int32
	 */
	required?: number;
};

export type Payment = {
	id?: string | null;
	/**
	 * @format date-time
	 */
	createdAt?: string;
	/**
	 * @format date-time
	 */
	updatedAt?: string;
	status?: PaymentStatus;
	type?: PaymentType;
	/**
	 * @format int32
	 */
	price?: number;
	description?: string | null;
	/**
	 * @format int32
	 */
	number?: number;
	userId?: string | null;
	user?: User;
};

export type PaymentDto = {
	status?: PaymentStatus;
	type?: PaymentType;
	/**
	 * @format int32
	 */
	price?: number;
	/**
	 * @format int32
	 */
	number?: number;
	description?: string | null;
	userId?: string | null;
};

/**
 * @format int32
 */
export type PaymentStatus = 0 | 1 | 2;

/**
 * @format int32
 */
export type PaymentType = 0 | 1;

export type Resource = {
	id?: string | null;
	/**
	 * @format date-time
	 */
	createdAt?: string;
	/**
	 * @format date-time
	 */
	updatedAt?: string;
	name?: string | null;
	/**
	 * @format int32
	 */
	amount?: number;
	unit?: Unit;
	comments?: string[] | null;
	commentsList?: Comment[] | null;
};

export type ResourceDto = {
	id?: string | null;
	name?: string | null;
	/**
	 * @format int32
	 */
	amount?: number;
	unit?: Unit;
};

export type Shift = {
	id?: string | null;
	/**
	 * @format date-time
	 */
	createdAt?: string;
	/**
	 * @format date-time
	 */
	updatedAt?: string;
	userId?: string | null;
	targetWeekId?: string | null;
	targetWeek?: Week;
	weeks?: string[] | null;
	weeksList?: Week[] | null;
};

/**
 * @format int32
 */
export type Unit = 0 | 1 | 2;

export type UpdateUserDto = {
	id?: string | null;
	fullName?: string | null;
	login?: string | null;
	role?: UserRole;
	status?: UserStatus;
};

export type User = {
	id?: string | null;
	/**
	 * @format date-time
	 */
	createdAt?: string;
	/**
	 * @format date-time
	 */
	updatedAt?: string;
	fullName?: string | null;
	login?: string | null;
	role?: UserRole;
	status?: UserStatus;
	shiftId?: string | null;
	shift?: Shift;
};

/**
 * @format int32
 */
export type UserRole = 0 | 1 | 2 | 3 | 4;

/**
 * @format int32
 */
export type UserStatus = 0 | 1 | 2 | 3;

export type Week = {
	id?: string | null;
	/**
	 * @format date-time
	 */
	createdAt?: string;
	/**
	 * @format date-time
	 */
	updatedAt?: string;
	/**
	 * @format int32
	 */
	monday?: number;
	/**
	 * @format int32
	 */
	tuesday?: number;
	/**
	 * @format int32
	 */
	wednesday?: number;
	/**
	 * @format int32
	 */
	thursday?: number;
	/**
	 * @format int32
	 */
	friday?: number;
	/**
	 * @format int32
	 */
	saturday?: number;
	/**
	 * @format int32
	 */
	sunday?: number;
	shiftId?: string | null;
	shift?: Shift;
};

export type WeekDto = {
	/**
	 * @format date-time
	 */
	createdAt?: string;
	monday?: string | null;
	tuesday?: string | null;
	wednesday?: string | null;
	thursday?: string | null;
	friday?: string | null;
	saturday?: string | null;
	sunday?: string | null;
	userId?: string | null;
};

export type СommentDto = {
	id?: string | null;
	content?: string | null;
	dishId?: string | null;
	resourceId?: string | null;
	userId?: string | null;
};
