import React, { useState } from 'react';
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
        const colors = ['red', 'orange', 'purple', 'blue'];
        return (
            datalist.map((d, idx) => {
                return (<Badge key={idx} style={{ backgroundColor: colors[idx] }} title={d.name} >{d.value}  </Badge>)
            })
        )

}
