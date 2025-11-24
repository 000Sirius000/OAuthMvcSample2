/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { Department } from '../models/Department';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class DepartmentsApiService {
    /**
     * @returns Department OK
     * @throws ApiError
     */
    public static getApiDepartmentsApi(): CancelablePromise<Array<Department>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/DepartmentsApi',
        });
    }
    /**
     * @param requestBody
     * @returns Department OK
     * @throws ApiError
     */
    public static postApiDepartmentsApi(
        requestBody?: Department,
    ): CancelablePromise<Department> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/DepartmentsApi',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param id
     * @returns Department OK
     * @throws ApiError
     */
    public static getApiDepartmentsApi1(
        id: number,
    ): CancelablePromise<Department> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/DepartmentsApi/{id}',
            path: {
                'id': id,
            },
        });
    }
    /**
     * @param id
     * @param requestBody
     * @returns any OK
     * @throws ApiError
     */
    public static putApiDepartmentsApi(
        id: number,
        requestBody?: Department,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/DepartmentsApi/{id}',
            path: {
                'id': id,
            },
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param id
     * @returns any OK
     * @throws ApiError
     */
    public static deleteApiDepartmentsApi(
        id: number,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'DELETE',
            url: '/api/DepartmentsApi/{id}',
            path: {
                'id': id,
            },
        });
    }
}
