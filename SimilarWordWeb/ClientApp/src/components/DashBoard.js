import React, { useState, Fragment } from 'react';
import { Badge } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { GetTokenHeader } from './CommTools';

export const DashBoard = ({ history }) => {
    const [firstFlag, setFirstFlag] = useState(0);
    const [datalist, setDatalist] = useState([]);


    const LoadData = () => {
        console.log("LoadData in Dashboard:");
        fetch('api/Dashboard', {
            headers: {
                'Content-Type': 'application/json',
                'Authorization': GetTokenHeader()
            }
        })
            .then(response => response.json())
            .then(data => {
                console.log('Dashboard fetch back');
                console.log(data);
                setFirstFlag(1);
                setDatalist(data);
            })
            .catch(err => console.log(`Error with message: ${err}`));
                //console.log('dashboard response.ok='+response.ok);
                //if (!response.ok) throw (new Error(`Failed:(${response.status}) ${response.statusText}`));

    }
    if (GetTokenHeader() === '') return '';

    if (firstFlag === 0) {
        console.log("will load Dashboard data");
        LoadData();
    }

        console.log("ShowDashBoard start:"+firstFlag);
        console.log(datalist);
    const colors = ['red', 'orange', 'purple', 'LimeGreen', 'LightGreen', 'blue'];
    if ( typeof datalist === 'undefined' ) return '';
    let len = datalist.length;

    return (
        <Fragment>
            <Link to={'/Wordmemory'} ><button className='btn btn-info btn-sm'> Start Study </button></Link>
            {len > 1 ? <Badge style={{ backgroundColor: 'red' }} title={'due today'} >{datalist[1].value}  </Badge> : ''}
            {' '}{len > 3 ? <Badge style={{ backgroundColor: 'orange' }} title={'New viewed today/new waiting'} >{datalist[3].value + ' / ' + datalist[0].value}  </Badge> : ''}
            {' '}{len > 4 ? <Badge style={{ backgroundColor: 'LimeGreen' }} title={'Viewed today/total viewed'} >{datalist[4].value + ' / ' + datalist[2].value+' / '+datalist[6].value+'d'}  </Badge> : ''}
            {' '}{len > 5 ? <Badge style={{ backgroundColor: 'blue' }} title={datalist[5].name} >{datalist[5].value}  </Badge> : ''}
           </Fragment>
        )

}
