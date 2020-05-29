import React, { useState } from 'react';
import { Link } from 'react-router-dom'
import { Badge } from 'react-bootstrap';
import { WordSearch } from './WordSearch';

const ShowDashBoard = ({ list }) => {
    console.log("ShowDashBoard start");
    console.log(list);
    const colors = ['red', 'orange', 'purple','blue' ];
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

export const Home = ({history}) => {
    const [firstFlag, setFirstFlag] = useState(-1);
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

    if (firstFlag === -1) {
        setFirstFlag(0);
        history.push('/WordSearch/');      // make sure the home address url has "/" at the end
    }

    if (firstFlag === 0) {
        console.log("Home will load data");
        LoadData();
    }

    // redirect to component "wordsearch"
        return (
            <div>
                <h3>Welcome!
            <ShowDashBoard list={datalist}></ShowDashBoard></h3>

                <input placeholder='search here' onChange={WordChanged} onKeyPress={KeyPressed} ></input>
                <Link to={{ pathname: '/wordsearch/' + wordInput }} > <button>Search</button> </Link>
            </div>
        );

}
