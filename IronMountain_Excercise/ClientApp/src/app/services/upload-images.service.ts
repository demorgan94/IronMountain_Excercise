import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { saveAs } from 'file-saver';
import { Image } from '../models/image.model';

@Injectable({
  providedIn: 'root'
})
export class UploadImagesService {
  apiUrl = '/api/ProcessImage';

  constructor(private _http: HttpClient) { }

  uploadImages(images: FormData): Observable<any> {
    return this._http.post(this.apiUrl, images);
  }

  downloadZip(imagesList: Image[]): Observable<any> {
    return this._http.post(this.apiUrl, imagesList);
  }
}
