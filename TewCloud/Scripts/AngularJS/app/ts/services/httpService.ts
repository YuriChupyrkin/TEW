import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import 'rxjs/add/operator/map';

@Injectable()
export class HttpService {

    constructor(private http: Http) { }

    public processGet<T>(url: string): Observable<T> {
        console.log(`get: ${url}`);

        return this.http.get(url).map(response => <T>response.json());
    }

    public processPost<T>(object: T, url: string) {
        console.log(`post: ${url}`);
        return this.http.post(url, object);

        //return this.http.post(url, object).map(j => j.json()).catch(this.handleError);
    }

    public processDelete<T>(object: T, url: string) {
        console.log(`delete: ${url}`);
        return this.http.delete(url, object);
    }

    private handleError() {
        console.log("ERRROR");
    }
}