/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { Department } from './Department';
import type { Position } from './Position';
export type Employee = {
    id?: number;
    fullName: string;
    email?: string | null;
    hireDate?: string;
    departmentId?: number;
    department?: Department;
    positionId?: number;
    position?: Position;
};

