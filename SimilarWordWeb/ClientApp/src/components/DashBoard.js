import React, { useState, Fragment } from 'react';
import { Badge } from 'react-bootstrap';


export const DashBoard = ({ history }) => {
    const [firstFlag, setFirstFlag] = useState(0);
    const [datalist, setDatalist] = useState([]);


    const LoadData = () => {
        console.log("LoadData in Dashboard:");
        fetch('api/Dashboard/')
            .then(response => response.json())
            .then(data => {
                console.log('fetch back');
                console.log(data);
                setFirstFlag(1);
                setDatalist(data);
            });
    }

    if (firstFlag === 0) {
        console.log("will load Dashboard data");
        LoadData();
    }

        console.log("ShowDashBoard start");
        console.log(datalist);
    const colors = ['red', 'orange', 'purple', 'LimeGreen', 'LightGreen', 'blue'];
    let len = datalist.length;
    return (
        <Fragment>
            {len > 1 ? <Badge style={{ backgroundColor: 'red' }} title={'due today'} >{datalist[1].value}  </Badge> : ''}
            {' '}{len > 3 ? <Badge style={{ backgroundColor: 'orange' }} title={'New viewed today/new waiting'} >{datalist[3].value + ' / ' + datalist[0].value}  </Badge> : ''}
            {' '}{len > 4 ? <Badge style={{ backgroundColor: 'LimeGreen' }} title={'Viewed today/total viewed'} >{datalist[4].value + ' / ' + datalist[2].value+' / '+datalist[6].value+'d'}  </Badge> : ''}
            {' '}{len > 5 ? <Badge style={{ backgroundColor: 'blue' }} title={datalist[5].name} >{datalist[5].value}  </Badge> : ''}
           </Fragment>
        )

}
