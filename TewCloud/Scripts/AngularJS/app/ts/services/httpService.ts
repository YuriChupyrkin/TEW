import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { ConstantStorage } from './constantStorage'

import 'rxjs/add/operator/map';

@Injectable()
export class HttpService {

    constructor(private http: Http) { }

    public processGet<T>(url: string, isExternalRequest = false): Observable<T> {
        console.log(`get: ${url}`);

        let headers = new Headers();
        let userId = ConstantStorage.getUserId()

        if (isExternalRequest == false && userId) {
            headers.append('Authorization', userId.toString());
        }

        return this.http.get(url, { headers: headers}).map(response => <T>response.json());
    }

    public processPost<T>(object: T, url: string) {
        console.log(`post: ${url}`);

        let headers = new Headers();

        let userId = ConstantStorage.getUserId()
        if (userId) {
            headers.append('Authorization', userId.toString());
        }

        return this.http.post(url, object, { headers: headers });
    }

    public processDelete<T>(object: T, url: string) {
        console.log(`delete: ${url}`);
        return this.http.delete(url, object);
    }

    private handleError() {
        console.log("ERRROR");
    }
}