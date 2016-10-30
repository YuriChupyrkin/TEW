import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { UserWords } from '../models/userWords';

import 'rxjs/add/operator/map';

@Injectable()
export class GetPostService {

    constructor(private http: Http) { }

    public getRequest<T>(url: string): Observable<T> {
        console.log(`GET REQUEST: ${url}`);
        return this.http.get(url)
            //.map(this.extractData);
            .map(response => <T>response.json());
    }

    private extractData(res: Response) {
        let body = res.json();
        return body.data || {};
    }
}