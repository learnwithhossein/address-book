import { Pagination } from './Pagination';

export class PagedResult<T>{
    body: T;
    pagination: Pagination
}