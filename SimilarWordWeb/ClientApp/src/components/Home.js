import React, { useState } from 'react';
import { Link } from 'react-router-dom'
import { Badge } from 'react-bootstrap';

const ShowDashBoard = ({ list }) => {
    console.log("ShowDashBoard start");
    console.log(list);
    const colors = ['red', 'blue', 'orange', 'black'];
    return (
        list.map((d, idx) => {
            return (<Badge key={idx} style={{ backgroundColor: colors[idx] }} title={d.name} >{d.value}  </Badge> )
        })
    )
}
const ShowDashBoardInTable = ({ list }) => {
    console.log("ShowDashBoard start");
    console.log(list);
    return (list.map((d, idx) => {
        return (
            <tr key={idx}>
                <td> {d.name}</td>
                <td> {d.value}</td>
            </tr >
        )
    })
    )
}

export const Home = () => {
    const [firstFlag, setFirstFlag] = useState(0);
    const [wordInput, setWordInput] = useState('');
    const [datalist, setDatalist] = useState([]);

    const WordChanged = (e) => {
        console.log('wordChanged');
        setWordInput(e.target.value);
        console.log(wordInput);
    }

    const KeyPressed = (event) => {
        console.log('KeyPressed');
        if (event.key === 'Enter') {
            console.log(wordInput);
            const { href } = window.location;
            window.location.href = `${href}wordsearch/${wordInput}`;
        }
    }


    const LoadData = () => {
        console.log("LoadData:");
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
        console.log("Home will load data");
        LoadData();
    }

    return (
      <div>
            <h3>Welcome!
            <ShowDashBoard list={datalist}></ShowDashBoard></h3>

            <input  placeholder='search here' onChange={WordChanged} onKeyPress={KeyPressed} ></input>
            <Link to={'/wordsearch/' + wordInput} > <button>Search</button> </Link>
      </div>
    );


}
